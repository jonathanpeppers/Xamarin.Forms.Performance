﻿using Android.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using AImageView = Android.Widget.ImageView;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(Image), typeof(xfperf.ImageRenderer))]

namespace xfperf
{
    internal sealed class ImageRenderer : AImageView, IVisualElementRenderer
    {
        bool _disposed;
        Image _element;
        bool _skipInvalidate;
        int? _defaultLabelFor;
        VisualElementTracker _visualElementTracker;

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                if (_visualElementTracker != null)
                {
                    _visualElementTracker.Dispose();
                    _visualElementTracker = null;
                }

                if (_element != null)
                {
                    _element.PropertyChanged -= OnElementPropertyChanged;
                }
            }

            base.Dispose(disposing);
        }

        public override void Invalidate()
        {
            if (_skipInvalidate)
            {
                _skipInvalidate = false;
                return;
            }

            base.Invalidate();
        }

        async void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            await TryUpdateBitmap(e.OldElement);
            UpdateAspect();
            this.EnsureId();

            ElementChanged?.Invoke(this, new VisualElementChangedEventArgs(e.OldElement, e.NewElement));
        }

        Size MinimumSize()
        {
            return new Size();
        }

        SizeRequest IVisualElementRenderer.GetDesiredSize(int widthConstraint, int heightConstraint)
        {
            if (_disposed)
            {
                return new SizeRequest();
            }

            Measure(widthConstraint, heightConstraint);
            return new SizeRequest(new Size(MeasuredWidth, MeasuredHeight), MinimumSize());
        }

        void IVisualElementRenderer.SetElement(VisualElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var image = element as Image;
            if (image == null)
                throw new ArgumentException("Element is not of type " + typeof(Image), nameof(element));

            Image oldElement = _element;
            _element = image;

            if (oldElement != null)
                oldElement.PropertyChanged -= OnElementPropertyChanged;

            element.PropertyChanged += OnElementPropertyChanged;

            if (_visualElementTracker == null)
                _visualElementTracker = new VisualElementTracker(this);

            OnElementChanged(new ElementChangedEventArgs<Image>(oldElement, _element));
        }

        void IVisualElementRenderer.SetLabelFor(int? id)
        {
            if (_defaultLabelFor == null)
                _defaultLabelFor = LabelFor;

            LabelFor = (int)(id ?? _defaultLabelFor);
        }

        void IVisualElementRenderer.UpdateLayout() => _visualElementTracker?.UpdateLayout();

        VisualElement IVisualElementRenderer.Element => _element;

        VisualElementTracker IVisualElementRenderer.Tracker => _visualElementTracker;

        AView IVisualElementRenderer.View => this;

        ViewGroup IVisualElementRenderer.ViewGroup => null;

        AImageView Control => this;

        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;
        public event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;

        public ImageRenderer() : base(Forms.Context)
        {
        }

        async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Image.SourceProperty.PropertyName)
                await TryUpdateBitmap();
            else if (e.PropertyName == Image.AspectProperty.PropertyName)
                UpdateAspect();

            ElementPropertyChanged?.Invoke(this, e);
        }

        async Task TryUpdateBitmap(Image previous = null)
        {
            // By default we'll just catch and log any exceptions thrown by UpdateBitmap so they don't bring down
            // the application; a custom renderer can override this method and handle exceptions from
            // UpdateBitmap differently if it wants to

            try
            {
                await UpdateBitmap(previous);
            }
            catch (Exception ex)
            {
                Log.Warning(nameof(ImageRenderer), "Error loading image: {0}", ex);
            }
            finally
            {
                ((IImageController)_element)?.SetIsLoading(false);
            }
        }

        async Task UpdateBitmap(Image previous = null)
        {
            if (_element == null || _disposed)
            {
                return;
            }

            await Control.UpdateBitmap(_element, previous);
        }

        void UpdateAspect()
        {
            if (_element == null || _disposed)
            {
                return;
            }

            ScaleType type = _element.Aspect.ToScaleType();
            SetScaleType(type);
        }
    }
}