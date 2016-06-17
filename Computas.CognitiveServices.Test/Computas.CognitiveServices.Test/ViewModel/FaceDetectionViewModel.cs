using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Computas.CognitiveServices.Test.Helper;
using Computas.CognitiveServices.Test.Models;
using FormsToolkit;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using MvvmHelpers;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Computas.CognitiveServices.Test.ViewModel
{
	public class FaceDetectionViewModel : ViewModelBase
	{
		private FaceServiceClient faceServiceClient;

		public FaceDetectionViewModel(INavigation navigation) : base(navigation)
		{
			faceServiceClient = new FaceServiceClient(App.FaceApiKey);
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
					imageFileSource = file.Path;
					var faces = await UploadAndDetectFaces(file);
					if (faces != null)
					{
						IsBusy = false;
						var faceList = new List<FaceView>();
						var index = 1;
						foreach (var face in faces)
						{
							if (face.FaceAttributes != null)
							{
								faceList.Add(new FaceView(index, face.FaceAttributes));
							}
							index++;
						}

						Faces.ReplaceRange(faceList);
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

		public ObservableRangeCollection<FaceView> Faces { get; } = new ObservableRangeCollection<FaceView>();

		private async Task ExecutePickPhotoCommandAsync()
		{
			IsBusy = true;
			if (CrossMedia.Current.IsPickPhotoSupported)
			{
				var file = await CrossMedia.Current.PickPhotoAsync();
				if (file != null)
				{
					ImageFileSource = ImageSource.FromStream(file.GetStream);
					var faces = await UploadAndDetectFaces(file);
					if (faces != null)
					{
						IsBusy = false;
						var faceList = new List<FaceView>();
						var index = 1;
						foreach (var face in faces)
						{
							if (face.FaceAttributes != null)
							{
								faceList.Add(new FaceView(index, face.FaceAttributes));
							}
							index++;
						}

						Faces.ReplaceRange(faceList);
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

		private async Task<Face[]> UploadAndDetectFaces(MediaFile file)
		{
			var faces = await faceServiceClient.DetectAsync(file.GetStream(), true, true, new FaceAttributeType[] {
                FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.FacialHair, FaceAttributeType.HeadPose,
				FaceAttributeType.Smile, FaceAttributeType.Glasses });

			return faces;
		}

		private async Task<Face[]> UploadAndFindFaceAttributes(MediaFile file)
		{
			try
			{
				var faceListName = Guid.NewGuid().ToString();
				await faceServiceClient.CreateFaceListAsync(faceListName, faceListName, "face list for sample");
				var faces = await faceServiceClient.DetectAsync(file.GetStream());
				foreach (var f in faces)
				{
					//var result = await faceServiceClient.FindSimilarAsync(f.FaceId, faces, 10);
				}


				return faces;
			}
			catch (Exception)
			{
			}

			return null;
		}
	}

	/// <summary>
	/// Find similar result for UI binding
	/// </summary>
	public class FindSimilarResult : INotifyPropertyChanged
	{
		#region Fields

		/// <summary>
		/// Similar faces collection
		/// </summary>
		private ObservableCollection<Face> _faces;

		/// <summary>
		/// Query face
		/// </summary>
		private Face _queryFace;

		#endregion Fields

		#region Events

		/// <summary>
		/// Implement INotifyPropertyChanged interface
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion Events

		#region Properties

		/// <summary>
		/// Gets or sets similar faces collection
		/// </summary>
		public ObservableCollection<Face> Faces
		{
			get { return _faces; }

			set
			{
				_faces = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("Faces"));
				}
			}
		}

		/// <summary>
		/// Gets or sets query face
		/// </summary>
		public Face QueryFace
		{
			get { return _queryFace; }

			set
			{
				_queryFace = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("QueryFace"));
				}
			}
		}

		#endregion Properties
	}
}