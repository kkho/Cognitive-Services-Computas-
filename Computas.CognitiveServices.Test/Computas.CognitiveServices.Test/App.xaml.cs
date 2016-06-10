using System;
using Computas.CognitiveServices.Test.Helper;
using Computas.CognitiveServices.Test.View;
using Computas.CognitiveServices.Test.ViewModel;
using FormsToolkit;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test
{
    public partial class App : Application
    {
		public static string VisionApiKey = "f1faa5e0029b49dda3fa0cfbc124b017";
		public static string EmotionApiKey = "1c4ae3bd14e24cc292bdb7de90913686";
		public static string SpeechApiKey = "38044ebb3282412292c5de88e17a0e8d";
		public static string FaceApiKey = "a1bba8636fc94e9b8199c002a61f8fad";

		public static App current;

		public App()
        {
			current = this;	
			InitializeComponent();
			ViewModelBase.Init();
			// The root page of your application
			switch (Device.OS)
			{
				case TargetPlatform.Android:
					MainPage = new RootPageAndroid();
					break;
				case TargetPlatform.iOS:
					MainPage = new CXCognitiveNavigationPage(new RootPageiOS());
					break;
				case TargetPlatform.Windows:
				case TargetPlatform.WinPhone:
					MainPage = new RootPageWindows();
					break;
				default:
					throw new NotImplementedException();
			}
		}

		public new static App Current
		{
			get
			{
				return (App)Application.Current;
			}
		}

		public INavigation Navigation { get; set; }

		protected override void OnSleep()
		{
			MessagingService.Current.Unsubscribe<MessagingServiceChoice>(MessagingKeys.VisionMessage);
			MessagingService.Current.Unsubscribe<MessagingServiceAlert>(MessagingKeys.CognitiveServiceErrorMessage);
		}

	}
}

