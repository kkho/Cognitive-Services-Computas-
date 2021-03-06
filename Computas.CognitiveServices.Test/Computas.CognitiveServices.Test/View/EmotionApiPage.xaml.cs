﻿using System;
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
	public partial class EmotionApiPage : ContentPage
	{
		EmotionApiViewModel ViewModel => vm ?? (vm = BindingContext as EmotionApiViewModel);
		EmotionApiViewModel vm;

		public EmotionApiPage()
		{
			InitializeComponent();
			BindingContext = vm = new EmotionApiViewModel(Navigation);
			MessagingService.Current.Subscribe<MessagingServiceAlert>(MessagingKeys.CognitiveServiceErrorMessage,
				(delegate(IMessagingService service, MessagingServiceAlert alert)
				{
					DisplayAlert(alert.Title, alert.Message, alert.Cancel);
				}));
		}
	}
}