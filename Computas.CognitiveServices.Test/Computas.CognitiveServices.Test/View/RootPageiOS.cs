using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Computas.CognitiveServices.Test.Util;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.View
{
	public class RootPageiOS : TabbedPage
	{
		public RootPageiOS()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			Children.Add(new CXCognitiveNavigationPage(new VisionApiPage()));
			// Children.Add(new CXCognitiveNavigationPage(new EmotionApiPage()));;
		}

		public void NavigateAsync(AppPage menuId)
		{
			switch ((int) menuId)
			{
				case (int) AppPage.Vision:
					CurrentPage = Children[0];
					break;
				case (int) AppPage.Emotion:
					CurrentPage = Children[1];
					break;
			}
		}
	}
}