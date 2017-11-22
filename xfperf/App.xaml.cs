using System;

using Xamarin.Forms;

namespace xfperf
{
	public partial class App : Application
	{
		public static bool UseMockDataStore = true;
		public static string BackendUrl = "https://localhost:5000";

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();

			if (Device.RuntimePlatform == Device.iOS)
				MainPage = new MainPage();
			else
				MainPage = new NavigationPage(new MainPage());
		}
	}
}
