using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Computas.CognitiveServices.Test.Helper;
using Computas.CognitiveServices.Test.ViewModel;
using FormsToolkit;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.View
{
	public partial class VisionApiPage : ContentPage
	{
		VisionApiViewModel ViewModel => vm ?? (vm = BindingContext as VisionApiViewModel);
		VisionApiViewModel vm;

		public VisionApiPage()
		{
			InitializeComponent();
			MessagingService.Current.Subscribe<MessagingServiceChoice>(MessagingKeys.VisionMessage,
				async (m, info) =>
				{
					var result = await DisplayActionSheet(info.Title, info.Cancel, null, info.Items);
					var indexOfPickedService = info.Items.ToList().FindIndex(v => v.Equals(result));
					await vm.PickedVisionService(indexOfPickedService);
				});

			BindingContext = vm = new VisionApiViewModel(Navigation);
		}
	}
}