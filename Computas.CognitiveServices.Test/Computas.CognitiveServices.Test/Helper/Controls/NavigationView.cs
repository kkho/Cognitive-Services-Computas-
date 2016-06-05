using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.Helper.Controls
{
	public class NavigationView : ContentView
	{
		public void OnNavigationItemSelected(NavigationItemSelectedEventArgs e)
		{
			NavigationItemSelected?.Invoke(this, e);
		}

		public event NavigationItemSelectedEventHandler NavigationItemSelected;

	}

	/// <summary>
	/// Arguments to pass to event handlers
	/// </summary>
	public class NavigationItemSelectedEventArgs : EventArgs
	{
		public int Index { get; set; }
	}

	public delegate void NavigationItemSelectedEventHandler(object sender, NavigationItemSelectedEventArgs e);

}
