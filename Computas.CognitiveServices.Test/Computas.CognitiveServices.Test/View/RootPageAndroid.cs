using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Computas.CognitiveServices.Test.Util;
using Computas.CognitiveServices.Test.View.Components;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.View
{
	public class RootPageAndroid : MasterDetailPage
	{
		Dictionary<int, CXCognitiveNavigationPage> pages;

		public RootPageAndroid()
		{
			pages = new Dictionary<int, CXCognitiveNavigationPage>();
			Master = new MenuPage(this);

			pages.Add(0, new CXCognitiveNavigationPage(new VisionApiPage()));

			Detail = pages[0];
		}

		public async Task NavigateAsync(int menuId)
		{
			CXCognitiveNavigationPage newPage = null;
			if (!pages.ContainsKey(menuId))
			{
				//only cache specific pages
				switch (menuId)
				{
					case (int) AppPage.Vision:
						pages.Add(menuId, new CXCognitiveNavigationPage(new VisionApiPage()));
						break;
					case (int) AppPage.Emotion:
						break;
					default:
						break;
				}
			}

			if (newPage == null)
				newPage = pages[menuId];

			if (newPage == null)
				return;

			//if we are on the same tab and pressed it again.
			if (Detail == newPage)
			{
				await newPage.Navigation.PopToRootAsync();
			}

			Detail = newPage;
		}
	}
}