using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion.Contract;

namespace Computas.CognitiveServices.Test.Models
{
	public class ScoresView
	{
		public ScoresView(int faceNumber, Scores scores)
		{
			FaceNumber = faceNumber + "";
			Anger = scores.Anger.ToString();
			Contempt = scores.Contempt.ToString();
			Disgust = scores.Disgust.ToString();
			Fear = scores.Fear.ToString();
			Happiness = scores.Happiness.ToString();
			Neutral = scores.Neutral.ToString();
			Sadness = scores.Sadness.ToString();
			Surprise = scores.Surprise.ToString();
		}

		public string faceNumber;

		public string FaceNumber
		{
			get { return faceNumber; }
			set { faceNumber = "Face: "+value; }
		}

		private string anger;

		public string Anger
		{
			get { return anger; }
			set { anger = "Anger: " + value; }
		}

		private string contempt;

		public string Contempt
		{
			get { return contempt; }
			set { contempt = "Contempt: " + value; }
		}

		private string disgust;

		public string Disgust
		{
			get { return disgust; }
			set { disgust = "Disgust: " + value; }
		}

		private string fear;

		public string Fear
		{
			get { return fear; }
			set { fear = "Fear: " + value; }
		}

		private string happiness;

		public string Happiness
		{
			get { return happiness; }
			set { happiness = "Happiness: " + value; }
		}

		private string neutral;

		public string Neutral
		{
			get { return neutral; }
			set { neutral = "Neutral: " + value; }
		}

		private string sadness;

		public string Sadness
		{
			get { return sadness; }
			set { sadness = "Sadness: " + value; }
		}

		private string surprise;

		public string Surprise
		{
			get { return surprise; }
			set { surprise = "Surprise: " + value; }
		}
	}
}