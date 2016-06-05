using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Computas.CognitiveServices.Test.Annotations;
using MvvmHelpers;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {

		protected INavigation Navigation { get; }

		public ViewModelBase(INavigation navigation = null)
		{
			Navigation = navigation;
		}

		public static void Init()
        {
            // DependencyService.Register<>();
            
        }
    }
}
