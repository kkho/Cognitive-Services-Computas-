using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
	public class VisionApiViewModel : ViewModelBase
	{
		private VisionServiceClient visionServiceClient;

		public VisionApiViewModel(INavigation navigation) : base(navigation)
		{
			visionServiceClient = new VisionServiceClient(App.VisionApiKey);
			SetupVisionApiUtilities();
		}

		private async void SetupVisionApiUtilities()
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
					// Do something with the photo here
				}

			}
		}

		private async Task<AnalysisResult> UploadAndDescribeImage(string imageFilePath)
		{

			using (Stream imageFileStream = File.OpenRead(imageFilePath))
			{
				// Upload and image and request three descriptions
				AnalysisResult analysisResult = await visionServiceClient.DescribeAsync(imageFileStream, 3);
				return analysisResult;
			}
		}

		private async Task<OcrResults> UploadAndRecognizeImage(string imageFilePath, string language)
		{
			using (Stream imageFileStream = File.OpenRead(imageFilePath))
			{
				OcrResults ocrResult = await visionServiceClient.RecognizeTextAsync(imageFileStream, language);
				return ocrResult;
			}
		}

		private async Task<AnalysisInDomainResult> UploadAndAnalyzeInDomainImage(string imageFilePath, Model domainModel)
		{
			using (Stream imageFileStream = File.OpenRead(imageFilePath))
			{
				AnalysisInDomainResult analysisResult = await visionServiceClient.AnalyzeImageInDomainAsync(imageFileStream, domainModel);
				return analysisResult;
			}
		}

		private async Task<AnalysisInDomainResult> AnalyzeInDomainUrl(string imageUrl, Model domainModel)
		{
			AnalysisInDomainResult analysisResult = await visionServiceClient.AnalyzeImageInDomainAsync(imageUrl, domainModel);
			return analysisResult;
		}

		private async Task<AnalysisResult> UploadAndGetTagsForImage(string imageFilePath)
		{
			using (Stream imageFileStream = File.OpenRead(imageFilePath))
			{
				AnalysisResult analysisResult = await visionServiceClient.GetTagsAsync(imageFileStream);
				return analysisResult;
			}
		}
		private async Task<byte[]> ThumbnailUrl(string imageUrl, int width, int height, bool smartCropping)
		{
			byte[] thumbnail = await visionServiceClient.GetThumbnailAsync(imageUrl, width, height, smartCropping);
			return thumbnail;
		}



		private async void OpenImageGallery()
		{
			if (CrossMedia.Current.IsPickPhotoSupported)
			{
				var photo = await CrossMedia.Current.PickPhotoAsync();
				// do something with the vision api here;
			}
		}
	}
}