using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Computas.CognitiveServices.Test.Util;
using MvvmHelpers;

namespace Computas.CognitiveServices.Test.Helper
{
	public class MenuItem : ObservableObject
	{
		string name;
		public string Name
		{
			get { return name; }
			set { SetProperty(ref name, value); }
		}
		string subtitle;
		public string Subtitle
		{
			get { return subtitle; }
			set { SetProperty(ref subtitle, value); }
		}

		public string Icon { get; set; }
		public string Parameter { get; set; }

		public AppPage Page { get; set; }
		public ICommand Command { get; set; }
	}
}
