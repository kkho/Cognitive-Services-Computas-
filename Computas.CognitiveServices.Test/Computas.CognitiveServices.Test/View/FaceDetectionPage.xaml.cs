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
	public partial class FaceDetectionPage : ContentPage
	{
		FaceDetectionViewModel ViewModel => vm ?? (vm = BindingContext as FaceDetectionViewModel);
		FaceDetectionViewModel vm;
		public FaceDetectionPage()
		{
			InitializeComponent();


			MessagingService.Current.Subscribe<MessagingServiceAlert>(MessagingKeys.CognitiveServiceErrorMessage,
				(delegate(IMessagingService service, MessagingServiceAlert alert)
				{
					DisplayAlert(alert.Title, alert.Message, alert.Cancel);
				}));

			BindingContext = vm = new FaceDetectionViewModel(Navigation);
		}
	}
}