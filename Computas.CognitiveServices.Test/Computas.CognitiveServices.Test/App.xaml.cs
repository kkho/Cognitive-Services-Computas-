using System;
using Computas.CognitiveServices.Test.View;
using Computas.CognitiveServices.Test.ViewModel;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test
{
    public partial class App : Application
    {
	    public static string VisionApiKey = "OWN API KEY";
	    public static string EmotionApiKey = "OWN API KEY";
	    public static string SpeechApiKey = "OWN API KEY";
	    public static string FaceApiKey = "OWN API KEY";

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

	}
}

