using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
	public class EmotionApiViewModel : ViewModelBase
	{
		private EmotionServiceClient emotionServiceClient;

		internal class EmotionResultDisplay
		{
			public string EmotionString { get; set; }
			public float Score { get; set; }
			public int OriginalIndex { get; set; }
		}

		public EmotionApiViewModel(INavigation navigation) : base(navigation)
		{
			emotionServiceClient = new EmotionServiceClient(App.EmotionApiKey);
		}

		private async Task<Emotion[]> UploadAndDetectEmotions(MediaFile file)
		{

			try
			{
				Emotion[] emotionResult;
					emotionResult = await emotionServiceClient.RecognizeAsync(file.GetStream());
					return emotionResult;
				
			}
			catch (Exception exception)
			{
				return null;
			}
		}

	}
}