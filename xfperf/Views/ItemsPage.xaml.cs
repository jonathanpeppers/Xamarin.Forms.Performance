using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace xfperf
{
	public partial class ItemsPage : ContentPage
	{
		public ItemsPage()
		{
			InitializeComponent();

            var grid = new Grid();
            grid.RowSpacing = 0;
            grid.ColumnSpacing = 0;
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < 100; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = 50 });

                for (int j = 0; j < 4; j++)
                {
                    var image = new Image
                    {
                        Source = ImageSource.FromFile($"patch{((j + i) % 4) + 1}")
                    };
                    Grid.SetRow(image, i);
                    Grid.SetColumn(image, j);
                    grid.Children.Add(image);
                }
            }
            _scroll.Content = grid;
		}
	}
}
