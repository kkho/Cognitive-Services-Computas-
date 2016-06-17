using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Computas.CognitiveServices.Test.Helper;
using Computas.CognitiveServices.Test.Models;
using Computas.CognitiveServices.Test.View;
using FormsToolkit;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
	public class EmotionApiViewModel : ViewModelBase
	{
		private EmotionServiceClient emotionServiceClient;

		public EmotionApiViewModel(INavigation navigation) : base(navigation)
		{
			emotionServiceClient = new EmotionServiceClient(App.EmotionApiKey);
		}

		internal class EmotionResultDisplay
		{
			public string EmotionString { get; set; }
			public float Score { get; set; }
			public int OriginalIndex { get; set; }
		}

		private ICommand takePhotoCommand;

		public ICommand TakePhotoCommand
			=> takePhotoCommand ?? (takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommandAsync()));

		private ICommand pickPhotoCommand;

		public ICommand PickPhotoCommand
			=> pickPhotoCommand ?? (pickPhotoCommand = new Command(async () => await ExecutePickPhotoCommandAsync()));

		private async Task ExecuteTakePhotoCommandAsync()
		{
			IsBusy = true;
			if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
			{
				// Supply media options for saving our photo after it's taken.
				var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					Directory = "CognitiveTest",
					Name = $"{DateTime.UtcNow}.jpg"
				};

				var file = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
				if (file != null)
				{
					imageFileSource = ImageSource.FromStream(file.GetStream);
					var emotionResult = await UploadAndDetectEmotions(file);
					if (emotionResult != null)
					{

						var scoreList = new List<ScoresView>();
						var faceNumber = 1;
						foreach (var emotion in emotionResult)
						{
							scoreList.Add(new ScoresView(faceNumber, emotion.Scores));
							faceNumber++;
						}

						Scores.ReplaceRange(scoreList);
						IsBusy = false;
					}
					else
					{
						MessagingService.Current.SendMessage<MessagingServiceAlert>(MessagingKeys.CognitiveServiceErrorMessage,
							new MessagingServiceAlert()
							{
								Cancel = "Cancel",
								Message = "Could not get " +
								          "data, please check your network",
								Title = "Error"
							});
					}
				}
			}
		}

		public ObservableRangeCollection<ScoresView> Scores { get; } = new ObservableRangeCollection<ScoresView>();

		private async Task ExecutePickPhotoCommandAsync()
		{
			IsBusy = true;
			if (CrossMedia.Current.IsPickPhotoSupported)
			{
				var file = await CrossMedia.Current.PickPhotoAsync();
				if (file != null)
				{
					ImageFileSource = ImageSource.FromStream(file.GetStream);
					var emotionResult = await UploadAndDetectEmotions(file);
					if (emotionResult != null)
					{
						var scoreList = new List<ScoresView>();
						var faceNumber = 1;
						foreach (var emotion in emotionResult)
						{
							
							scoreList.Add(new ScoresView(faceNumber, emotion.Scores));
							faceNumber++;
						}

						Scores.ReplaceRange(scoreList);
						IsBusy = false;
					}
					else
					{
						MessagingService.Current.SendMessage<MessagingServiceAlert>(MessagingKeys.CognitiveServiceErrorMessage,
							new MessagingServiceAlert()
							{
								Cancel = "Cancel",
								Message = "Could not get " +
								          "data, please check your network",
								Title = "Error"
							});
					}
				}
			}
		}

		private ImageSource imageFileSource;

		public ImageSource ImageFileSource
		{
			get { return imageFileSource; }
			set
			{
				imageFileSource = value;
				OnPropertyChanged();
			}
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