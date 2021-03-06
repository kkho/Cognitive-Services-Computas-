﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Computas.CognitiveServices.Test.Helper;
using FormsToolkit;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using MvvmHelpers;
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
					tempFileWorking = file;
					CallDisplaySheetOnView();
				}
			}
		}

		private async Task ExecutePickPhotoCommandAsync()
		{
			IsBusy = true;
			if (CrossMedia.Current.IsPickPhotoSupported)
			{
				tempFileWorking = await CrossMedia.Current.PickPhotoAsync();
				if (tempFileWorking != null)
				{
					CallDisplaySheetOnView();
				}
			}
		}

		private void CallDisplaySheetOnView()
		{
			var values = new[]
			{
				"Upload and Describe Image",
				"Upload and Recognize Image (OCR)",
				"Upload and Get Tags for Image",
                "Upload and analyze image"
			};
			MessagingService.Current.SendMessage<MessagingServiceChoice>(MessagingKeys.VisionMessage,
				new MessagingServiceChoice
				{
					Cancel = "Cancel",
					Destruction = null,
					Items = values,
					Title = "What Vision Service should you use?"
				});
		}

		public async Task PickedVisionService(int serviceType)
		{
			bool completed = false;
			switch (serviceType)
			{
				case (int) VisionApiServices.DescribeImage:
					AnalysisResult analyzeDescriptionImage = await UploadAndDescribeImage(tempFileWorking);
					if (analyzeDescriptionImage != null
					    && analyzeDescriptionImage.Description != null)
					{
						CaptionCollection.ReplaceRange(analyzeDescriptionImage.Description.Captions);
						TagCollection.ReplaceRange(analyzeDescriptionImage.Description.Tags);
						WriteCaptionResult();
						WriteTagResults();
						completed = true;
					}

					break;
				case (int) VisionApiServices.RecognizeImage:
					CultureInfo getCultureInfo = CultureInfo.CurrentCulture;
					OcrResults ocrResults = await UploadAndRecognizeImage(tempFileWorking, "no");
					if (ocrResults != null && ocrResults.Regions != null &&
					    ocrResults.Regions.Any())
					{
						FillOCRInformation(ocrResults.Regions);
						completed = true;
					}
					break;
				case (int) VisionApiServices.GetTags:
					var analysis = await UploadAndGetTagsForImage(tempFileWorking);
					if (analysis != null && analysis.Tags != null && analysis.Tags.Any())
					{
						var tagNames = analysis.Tags.Select(t => t.Name).ToList();
						TagCollection.ReplaceRange(tagNames);
						WriteTagResults();
						completed = true;
					}

					break;
                case (int)VisionApiServices.GetAnalysis:
			        var analyzeImage = await AnalyzeImage(tempFileWorking);
			        if (analyzeImage != null)
			        {
			            var adult = analyzeImage.Adult != null
			                ? $"Adult: {analyzeImage.Adult.AdultScore} score, " +
			                  $"{analyzeImage.Adult.IsAdultContent} content," +
			                  $" {analyzeImage.Adult.IsRacyContent} racy content, " +
			                  $"{analyzeImage.Adult.RacyScore} score\n\n"
			                : "no adult content\n";
			            var description = analyzeImage.Description != null
			                ? $"Description: TAGS: {WriteTagResultsDescription(analyzeImage.Description.Tags)}\n" +
			                  $"CAPTION: {analyzeImage.Description.Captions}\n" +
			                  $"Color: {analyzeImage.Color}\n":
                              "no description\n";
			            AnalysisResult = adult + description;
			        }
                    break;
                default:
					break;
			}

			if (!completed)
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

			IsBusy = false;
		}

        private string WriteTagResultsDescription(string []tags)
        {
            string formattedTags = string.Empty;
            var results = tags;
            for (int i = 0; i < results.Length; i++)
            {
                formattedTags += results[i];
                if (i != tags.Count() - 1)
                    formattedTags += ", ";
            }

            return formattedTags;
        }

        private void FillOCRInformation(IEnumerable<Region> regions)
		{
			string words = string.Empty;
			foreach (var region in regions)
			{
				for (var i = 0; i < region.Lines.Count(); i++)
				{
					for (var j = 0; j < region.Lines[i].Words.Count(); j++)
					{
						words += region.Lines[i].Words[j].Text + " ";
						if (i == region.Lines[i].Words.Count() - 1)
						{
							words += "\n";
						}
					}
				}
			}

			VisionMainResults = words;
		}


		private string _visionMainResults;

		public string VisionMainResults
		{
			get { return _visionMainResults; }

			set
			{
				_visionMainResults = value;
				OnPropertyChanged();
			}
		}

	    private string _analysisResult;

	    public string AnalysisResult
	    {
	        get { return _analysisResult; }
	        set
	        {
	            _analysisResult = value;
	            OnPropertyChanged();
	        }
	    }


        private void WriteCaptionResult()
		{
			string formattedCaption = string.Empty;
			var results = CaptionCollection.ToArray();
			for (int i = 0; i < results.Length; i++)
			{
				formattedCaption += results[i].Text + ", Confidence: " + results[i].Confidence;
				if (i != CaptionCollection.Count - 1)
					formattedCaption += "\n ";
			}

			VisionMainResults = formattedCaption;
		}

		private string tagResults;

		public string TagResults
		{
			get { return tagResults; }

			set
			{
				tagResults = value;
				OnPropertyChanged();
			}
		}

		private void WriteTagResults()
		{
			string formattedTags = string.Empty;
			var results = TagCollection.ToArray();
			for (int i = 0; i < results.Length; i++)
			{
				formattedTags += results[i];
				if (i != TagCollection.Count - 1)
					formattedTags += ", ";
			}

			TagResults = formattedTags;
		}

		public ObservableRangeCollection<Caption> CaptionCollection { get; } = new ObservableRangeCollection<Caption>();
		public ObservableRangeCollection<string> TagCollection { get; } = new ObservableRangeCollection<string>();

		private async Task<AnalysisResult> UploadAndDescribeImage(MediaFile file)
		{
			IsBusy = true;
			AnalysisResult analysisResult = await visionServiceClient.DescribeAsync(file.GetStream(), 3);
			return analysisResult;
		}


		private async Task<OcrResults> UploadAndRecognizeImage(MediaFile file, string langCode)
		{
			OcrResults ocrResult = await visionServiceClient.RecognizeTextAsync(file.GetStream());
			return ocrResult;
		}

		private async Task<AnalysisInDomainResult> UploadAndAnalyzeImage(MediaFile file, Model domainModel)
		{
			AnalysisInDomainResult analysisResult =
				await visionServiceClient.AnalyzeImageInDomainAsync(file.GetStream(), domainModel);
			return analysisResult;
		}

		private async Task<AnalysisResult> AnalyzeImage(MediaFile file)
		{
			AnalysisResult analysisResult = await visionServiceClient.AnalyzeImageAsync(file.GetStream(), new List<VisualFeature>()
			{
			    VisualFeature.Adult, VisualFeature.Categories, VisualFeature.Color, VisualFeature.Description, VisualFeature.Faces, VisualFeature.ImageType
			});
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