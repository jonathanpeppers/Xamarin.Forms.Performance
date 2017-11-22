using Android.App;
using Android.Content.PM;
using Android.OS;
using xfperf.Services;

namespace xfperf.Droid
{
	[Activity(Label = "xfperf.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			Profiler.Start("Forms.Init");
			global::Xamarin.Forms.Forms.Init(this, bundle);
			Profiler.Stop("Forms.Init");

			Profiler.Start("LoadApplication");
			LoadApplication(new App());
			Profiler.Stop("LoadApplication");
		}

		protected override void OnResume()
		{
			base.OnResume();

			Profiler.Stop("OnResume");
		}
	}
}
