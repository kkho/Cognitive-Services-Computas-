using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.View
{
	public class CXCognitiveNavigationPage : NavigationPage
	{
		public CXCognitiveNavigationPage(Page root) : base(root)
        {
			Init();
			Title = root.Title;
			Icon = root.Icon;
		}

		public CXCognitiveNavigationPage()
		{
			Init();
		}

		void Init()
		{
			if (Device.OS == TargetPlatform.iOS)
			{
				BarBackgroundColor = Color.FromHex("FAFAFA");
			}
			else
			{
				BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
				BarTextColor = (Color)Application.Current.Resources["NavigationText"];
			}
		}
	}
}

