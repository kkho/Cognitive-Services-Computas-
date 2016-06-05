using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Computas.CognitiveServices.Test.Helper;
using Computas.CognitiveServices.Test.ViewModel;
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
			MessagingCenter.Subscribe<string>(this, MessagingKeys.VisionMessage, async(values) =>
			{
				var pickedVisionService = await DisplayActionSheet("What Vision Service should you use?", "Cancel",  null, values);
				if (pickedVisionService != null)
				{
					var indexOfPickedService = values.IndexOf(pickedVisionService);
					await vm.PickdVisionService(indexOfPickedService);
				}


			});
			BindingContext = vm = new VisionApiViewModel(Navigation);
		}
	}
}
