using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Computas.CognitiveServices.Test.Util;
using Computas.CognitiveServices.Test.View.Components;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.View
{
	public class RootPageWindows : MasterDetailPage
	{
		Dictionary<AppPage, Page> pages;
		MenuPageUWP menu;
		public static bool IsDesktop { get; set; }

		public RootPageWindows()
		{
			//MasterBehavior = MasterBehavior.Popover;
			pages = new Dictionary<AppPage, Page>();

			var items = new ObservableCollection<Computas.CognitiveServices.Test.Helper.MenuItem>
			{
				new Computas.CognitiveServices.Test.Helper.MenuItem {Name = "Vision", Page = AppPage.Vision},
				new Computas.CognitiveServices.Test.Helper.MenuItem {Name = "Emotion", Page = AppPage.Emotion},
				new Computas.CognitiveServices.Test.Helper.MenuItem {Name = "Face", Page = AppPage.Face},
				new Computas.CognitiveServices.Test.Helper.MenuItem {Name = "Speech", Page = AppPage.Speech}
			};

			menu = new MenuPageUWP();
			menu.MenuList.ItemsSource = items;


			menu.MenuList.ItemSelected += (sender, args) =>
			{
				if (menu.MenuList.SelectedItem == null)
					return;

				Device.BeginInvokeOnMainThread(() =>
				{
					NavigateAsync(((Computas.CognitiveServices.Test.Helper.MenuItem) menu.MenuList.SelectedItem).Page);
					if (!IsDesktop)
						IsPresented = false;
				});
			};

			Master = menu;
			NavigateAsync((int) AppPage.Vision);
			Title = "Cognitive Services";
		}


		public void NavigateAsync(AppPage menuId)
		{
			Page newPage = null;
			if (!pages.ContainsKey(menuId))
			{
				//only cache specific pages
				switch (menuId)
				{
					case AppPage.Vision:
						pages.Add(menuId, new CXCognitiveNavigationPage(new VisionApiPage()));
						break;
					case AppPage.Emotion:
						pages.Add(menuId, new CXCognitiveNavigationPage(new EmotionApiPage()));
						break;
					case AppPage.Face:
						pages.Add(menuId, new CXCognitiveNavigationPage(new FaceDetectionPage()));
						break;
				}
			}

			if (newPage == null)
				newPage = pages[menuId];

			if (newPage == null)
				return;

			Detail = newPage;
			//await Navigation.PushAsync(newPage);
		}
	}
}