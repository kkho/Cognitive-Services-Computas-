using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Computas.CognitiveServices.Test.Annotations;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
