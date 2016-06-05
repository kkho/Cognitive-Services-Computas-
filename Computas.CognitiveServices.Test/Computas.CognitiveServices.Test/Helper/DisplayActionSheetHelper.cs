using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.Helper
{
	public class DisplayActionSheetHelper
	{
		public static async void OnActionSheetShowVisionApiPicker(string[] values)
		{
			MessagingCenter.Send(MessagingKeys.VisionMessage, "ShowActionSheet", values);
		}
	}
}
