using System;
using Android.Runtime;
using Android.App;
using xfperf.Services;

namespace xfperf.Droid
{
	[Application]
	public class Application : Android.App.Application
	{
		public Application(IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
		}

		public override void OnCreate()
		{
			Profiler.Start("OnResume");

			base.OnCreate();
		}
	}
}
