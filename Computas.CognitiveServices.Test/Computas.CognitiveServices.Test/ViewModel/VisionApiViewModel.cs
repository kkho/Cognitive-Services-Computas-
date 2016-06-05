using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Computas.CognitiveServices.Test.Helper;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Xamarin.Forms;
using Plugin.Media.Abstractions;

namespace Computas.CognitiveServices.Test.ViewModel
{
	public class VisionApiViewModel : ViewModelBase
	{
		private VisionServiceClient visionServiceClient;

		private MediaFile tempFileWorking;

		public VisionApiViewModel(INavigation navigation) : base(navigation)
		{
			visionServiceClient = new VisionServiceClient(App.VisionApiKey);
			SetupVisionApiUtilities();
		}

		private ICommand TakePhotoCommand { get; set; }
		private ICommand PickPhotoCommand { get; set; }

		private void SetupVisionApiUtilities()
		{
			TakePhotoCommand = new Command(async () =>
			{
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
						tempFileWorking = file;
						// Do something with the photo here
						var analysisResult = await UploadAndDescribeImage(file);
					}
				}
			});

			PickPhotoCommand = new Command(async () =>
			{
				if (CrossMedia.Current.IsPickPhotoSupported)
				{
					tempFileWorking = await CrossMedia.Current.PickPhotoAsync();
				}
			});
		}

		public async Task PickdVisionService(int serviceType)
		{
			switch (serviceType)
			{
				case (int)VisionApiServices.DescribeImage:
					AnalysisResult analyzeDescriptionImage  = await UploadAndDescribeImage(tempFileWorking);



					break;
				default:
					break;
			}

			IsBusy = false;
		}

		private async Task<AnalysisResult> UploadAndDescribeImage(MediaFile file)
		{
			IsBusy = true;
			AnalysisResult analysisResult = await visionServiceClient.DescribeAsync(file.GetStream(), 3);
			return analysisResult;
		}


		private async Task<OcrResults> UploadAndRecognizeImage(MediaFile file, string langCode)
		{
			OcrResults ocrResult = await visionServiceClient.RecognizeTextAsync(file.GetStream(), langCode);
			return ocrResult;
		}

		private async Task<AnalysisInDomainResult> UploadAndAnalyzeInDomainImage(MediaFile file, Model domainModel)
		{
			AnalysisInDomainResult analysisResult =
				await visionServiceClient.AnalyzeImageInDomainAsync(file.GetStream(), domainModel);
			return analysisResult;
		}

		private async Task<AnalysisInDomainResult> AnalyzeInDomainUrl(string imageUrl, Model domainModel)
		{
			AnalysisInDomainResult analysisResult = await visionServiceClient.AnalyzeImageInDomainAsync(imageUrl, domainModel);
			return analysisResult;
		}

		private async Task<AnalysisResult> UploadAndGetTagsForImage(MediaFile file)
		{
			AnalysisResult analysisResult = await visionServiceClient.GetTagsAsync(file.GetStream());
			return analysisResult;
		}

		private async Task<byte[]> ThumbnailUrl(string imageUrl, int width, int height, bool smartCropping)
		{
			byte[] thumbnail = await visionServiceClient.GetThumbnailAsync(imageUrl, width, height, smartCropping);
			return thumbnail;
		}
	}
}