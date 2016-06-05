using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Computas.CognitiveServices.Test.Droid.Service;
using Computas.CognitiveServices.Test.Helper.Controls;
using Computas.CognitiveServices.Test.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NavigationView = Android.Support.Design.Widget.NavigationView;

[assembly: ExportRenderer(typeof(Computas.CognitiveServices.Test.Helper.Controls.NavigationView), typeof(NavigationViewRenderer))]
namespace Computas.CognitiveServices.Test.Droid.Service
{
	public class NavigationViewRenderer :ViewRenderer<Computas.CognitiveServices.Test.Helper.Controls.NavigationView, NavigationView>
	{
		NavigationView navView;
		ImageView profileImage;
		TextView profileName;
		protected override void OnElementChanged(ElementChangedEventArgs<Computas.CognitiveServices.Test.Helper.Controls.NavigationView> e)
		{

			base.OnElementChanged(e);
			if (e.OldElement != null || Element == null)
				return;


			var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
			navView = view.JavaCast<NavigationView>();


			navView.NavigationItemSelected += NavView_NavigationItemSelected;

			SetNativeControl(navView);

			var header = navView.GetHeaderView(0);
			navView.SetCheckedItem(Resource.Id.nav_vision);
		}

		public override void OnViewRemoved(Android.Views.View child)
		{
			base.OnViewRemoved(child);
			navView.NavigationItemSelected -= NavView_NavigationItemSelected;
		}

		IMenuItem previousItem;

		void NavView_NavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			if (previousItem != null)
				previousItem.SetChecked(false);

			navView.SetCheckedItem(e.MenuItem.ItemId);

			previousItem = e.MenuItem;

			int id = 0;
			switch (e.MenuItem.ItemId)
			{
				case Resource.Id.nav_vision:
					id = (int)AppPage.Vision;
					break;
				case Resource.Id.nav_emotion:
					id = (int)AppPage.Emotion;
					break;
			}
			this.Element.OnNavigationItemSelected(new NavigationItemSelectedEventArgs
			{

				Index = id
			});
		}


	}
}