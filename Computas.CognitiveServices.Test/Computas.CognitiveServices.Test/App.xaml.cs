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


        public static string VisionApiKey = "OWN KEY";
        public static string EmotionApiKey = "OWN KEY";
        public static string SpeechApiKey = "OWN KEY";
        public static string FaceApiKey = "OWN KEY";

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

        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnSleep()
		{
		}

	}
}

