using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class AutoMatchScoutingPage:ContentPage
	{
		ParseObject data;

		Label TotalPoints = new Label ();

		int autoPoints = 0, robotSet = 0, containerSet = 0, toteSet =0, stackedToteSet = 0;

		string errorString = "The following data was unable to be saved: ";
		bool errorStatus = false;

		public AutoMatchScoutingPage (ParseObject MatchData)
		{	
			//Robot Set
			Button RobotSetBtn = new Button(){
				Text = "Robot Set",
				BackgroundColor = Color.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			RobotSetBtn.Clicked += (object sender, EventArgs e) => {
				if(robotSet==0){
					robotSet=1;
					RobotSetBtn.BackgroundColor = Color.Green;
					UpdateValues();
				} else {
					robotSet=0;
					RobotSetBtn.BackgroundColor = Color.Gray;
					UpdateValues();
				}
			};

			//Container Set
			Button ContainerSetBtn = new Button(){
				Text = "Container Set",
				BackgroundColor = Color.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			ContainerSetBtn.Clicked += (object sender, EventArgs e) => {
				if(containerSet==0){
					containerSet=1;
					ContainerSetBtn.BackgroundColor = Color.Green;
					UpdateValues();
				} else {
					containerSet=0;
					ContainerSetBtn.BackgroundColor = Color.Gray;
					UpdateValues();
				}
			};

			//Stacked Tote Set
			Button StackedToteSetBtn = new Button(){
				Text = "Stacked Tote Set",
				BackgroundColor = Color.Red,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			StackedToteSetBtn.Clicked += (object sender, EventArgs e) => {
				if(stackedToteSet==0 && toteSet ==1){
					stackedToteSet=1;
					StackedToteSetBtn.BackgroundColor = Color.Green;
					UpdateValues();
				} else if(stackedToteSet==1 && stackedToteSet==1) {
					stackedToteSet=0;
					StackedToteSetBtn.BackgroundColor = Color.Gray;
					UpdateValues();
				}
			};

			//Tote Set
			Button ToteSetBtn = new Button (){
				Text = "Tote Set",
				BackgroundColor = Color.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			ToteSetBtn.Clicked += (object sender, EventArgs e) => {
				if(toteSet==0){
					toteSet=1;
					ToteSetBtn.BackgroundColor = Color.Green;
					StackedToteSetBtn.BackgroundColor = Color.Gray;
					UpdateValues();
				} else {
					toteSet=0;
					stackedToteSet=0;
					ToteSetBtn.BackgroundColor = Color.Gray;
					StackedToteSetBtn.BackgroundColor = Color.Red;
					UpdateValues();
				}
			};

			//Pull Can from Step
			Label autoCansLabel = new Label {
				Text = "Step Cans Pulled in Auto:"
			};

			Picker autoCans = new Picker();
			for(int i = 1; i<=5; i++){
				autoCans.Items.Add(Convert.ToString(i));
			}
			autoCans.Title = "0";
			autoCans.SelectedIndexChanged += (sender, args) => {
				autoCans.Title = Convert.ToString(autoCans.SelectedIndex+1);
			};

			data = MatchData;

			Button TeleopPage = new Button (){
				Text = "TeleOp",
				BackgroundColor = Color.Yellow,
				TextColor = Color.Black,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};
			TeleopPage.Clicked += (object sender, EventArgs e) => {
				UpdateValues();
				errorHandling("autoPoints", autoPoints);
				if(string.IsNullOrEmpty(autoCans.Title) == false)
					errorHandling("autoStepCanPulls", Convert.ToInt32(autoCans.Title));

				if(errorStatus == true){
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
				} else {
					Navigation.PushModalAsync(new TeleOpMatchScoutingPage(data));
				}

				/*
				data["autoPoints"] = autoPoints;
				if(string.IsNullOrEmpty(autoCans.Title)==false){
					data["autoStepCanPulls"] = Convert.ToInt16(autoCans.Title);
				}
				SaveData();
				Navigation.PushModalAsync(new TeleOpMatchScoutingPage(MatchData));
				*/
			};

			this.Content = new StackLayout {
				Children = {
					TotalPoints,
					RobotSetBtn,
					ContainerSetBtn,
					ToteSetBtn,
					StackedToteSetBtn,
					autoCansLabel,
					autoCans,
					TeleopPage
				}
			};
		}

		void errorHandling(string d, int i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}

		async void UpdateValues(){
			autoPoints = (robotSet*4)+(containerSet*8)+(toteSet*6)+(stackedToteSet*14);
			TotalPoints.Text = autoPoints.ToString();
		}
	}
}

