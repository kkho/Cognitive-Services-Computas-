using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			BindingContext = vm = new VisionApiViewModel(Navigation);
		}
	}
}
