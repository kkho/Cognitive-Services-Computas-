using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;

namespace Computas.CognitiveServices.Test.Models
{
	public class FaceView
	{
		private FaceAttributes faceAttributes;

		public FaceView(int faceNumber, FaceAttributes faceAttributes)
		{
			FaceNumber = "Face: " + faceNumber;
			Age = "Age: " + faceAttributes.Age;
			FacialHair = faceAttributes.FacialHair != null? "Facial Hair: " + "Beard: " + faceAttributes.FacialHair.Beard + ", Moustache: "
				+ faceAttributes.FacialHair.Moustache + ", Sideburns: " + faceAttributes.FacialHair.Sideburns : " No facial hair??";
			Gender = "Gender: " + faceAttributes.Gender;
			Glasses = "Glasses: " + faceAttributes.Glasses != null ?  GlassType(faceAttributes.Glasses) : "No Glasses";
			HeadPose = "HeadPose: " + faceAttributes.HeadPose != null ? HeadPoseInformation(faceAttributes.HeadPose) : "No Headpose?";
			Smile = "Smile: " + faceAttributes.Smile;
		}

		public string FaceNumber { get; set; }

		public string Age { get; set; }
		public string FacialHair { get; set; }
		public string Gender { get; set; }
		public string Glasses { get; set; }
		public string HeadPose { get; set; }
		public string Smile { get; set; }

		private string GlassType(Glasses glasses)
		{
			var glassInformation = "Has: ";
			if (glasses == Microsoft.ProjectOxford.Face.Contract.Glasses.NoGlasses)
			{
				glassInformation += "No glasses ";
			}
			if (glasses == Microsoft.ProjectOxford.Face.Contract.Glasses.ReadingGlasses)
			{
				glassInformation += "Reading glasses ";
			}
			if (glasses == Microsoft.ProjectOxford.Face.Contract.Glasses.Sunglasses)
			{
				glassInformation += "Sunglasses ";
			}
			if (glasses == Microsoft.ProjectOxford.Face.Contract.Glasses.SwimmingGoggles)
			{
				glassInformation += "Swimming googgles";
			}

			return glassInformation;
		}

		private string HeadPoseInformation(HeadPose headPose)
		{
			return "HeadPose: " +"Pitch: " + headPose.Pitch + ", " + "Roll" + headPose.Roll + ", Yaw" +
				headPose.Yaw;
		}
	}
}
