using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computas.CognitiveServices.Test.Util
{
	public class DeepLinkPage
	{
		public AppPage Page { get; set; }
		public string Id { get; set; }
	}
	public enum AppPage
	{
		Vision, 
		Emotion,
		Face,
		Speech
	}
}
