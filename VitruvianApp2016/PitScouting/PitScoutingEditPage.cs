using System;
using Xamarin.Forms;
using Parse;
using Xamarin.Media;
using System.IO;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class PitScoutingEditPage:ContentPage
	{
		ParseObject data;

		enum DriveTypes {Tank, WestCoast, Holonomic, Omni, Mechanum, Other};
		enum Choice {Yes, No};

		const string errorStringDefault = "The Following Data Was Unable To Be Saved: ";
		string errorString = errorStringDefault;
		bool error = false;

		Image robotImage = new Image();
		MediaFile robotImageFile;

		public PitScoutingEditPage (ParseObject teamData)
		{
			data = teamData;

			Grid grid = new Grid () {
				//Padding = new Thickness(0,20,0,0), 
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = new GridLength(160, GridUnitType.Absolute) },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = new GridLength(160, GridUnitType.Absolute) },
					new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) }
				}
			};

			addRobotImage ();

			string teamNo;
			try{
				teamNo = teamData ["teamNo"].ToString ();
			}
			catch{
				teamNo = "<No Data Recorded>";
			}
			Label title = new Label {
				Text = teamNo + "'s Stats"
			};

			Title = title.Text;

			// Team Number Label
			Label teamNumber = new Label ();
			try {
				if (teamData ["teamNumber"] != null) {
					teamNumber.Text = teamData ["teamNumber"].ToString();
				} else {}
			}
			catch {
				teamNumber.Text = "<No Team Number>";
			}
			teamNumber.FontSize = GlobalVariables.sizeTitle;

			// Team Name Label
			Label teamName = new Label ();
			try {
				if (teamData ["teamName"] != null) {
					teamName.Text = teamData ["teamName"].ToString();
				} else {} 
			} catch {
				teamName.Text = "<No Team Name>";
			}
			teamName.FontSize = GlobalVariables.sizeMedium;

			// Drive Type Picker
			Label driveTypeLabel = new Label {
				Text = "Drive Type",
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker driveTypePicker = new Picker();
			try{
				if(teamData["driveType"] != null)
					driveTypePicker.Title = teamData["driveType"].ToString();
			}
			catch{
				driveTypePicker.Title = "Choose an Option";
			}
			for (DriveTypes i = DriveTypes.Tank; i <= DriveTypes.Other; i++) {
				driveTypePicker.Items.Add (i.ToString ());
			};
			driveTypePicker.SelectedIndexChanged += (sender, e) => {
				DriveTypes type = (DriveTypes)driveTypePicker.SelectedIndex;
				driveTypePicker.Title = type.ToString ();
			};

			// Low Bar Picker
			Label lowBarLabel = new Label {
				Text = "Can you go under the low bar?:",
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker lowBarPicker = new Picker();
			try{
				if(teamData["lowBarAccess"] != null)
					lowBarPicker.Title = teamData["lowBarAccess"].ToString();
			}
			catch{
				lowBarPicker.Title = "Choose an Option";
			}
			for (Choice i = Choice.Yes; i <= Choice.No; i++) {
				lowBarPicker.Items.Add (i.ToString ());
			};
			lowBarPicker.SelectedIndexChanged += (sender, e) => {
				Choice type = (Choice)lowBarPicker.SelectedIndex;
				lowBarPicker.Title = type.ToString();
			};

			// Auto Strategy Editor
			Label autoStrategyLabel = new Label {
				Text = "Auto Strategy",
				FontSize = GlobalVariables.sizeMedium,
			};

			Editor autoStrategyEditor = new Editor(){
				HeightRequest = 200,
				BackgroundColor = Color.Gray
			};
			try{
				if(teamData["autoStrategy"] != null)
					autoStrategyEditor.Text = data ["autoStrategy"].ToString();
			} catch {
				autoStrategyEditor.Text = "<No Data Recorded>";
			}

			// TeleOp Strategy Editor
			Label teleOpStrategyLabel = new Label {
				Text = "teleOp Strategy",
				FontSize = GlobalVariables.sizeMedium,
			};

			Editor teleOpStrategyEditor = new Editor(){
				HeightRequest = 200,
				BackgroundColor = Color.Gray
			};
			try{
				if(teamData["teleOpStrategy"] != null)
					teleOpStrategyEditor.Text = data ["teleOpStrategy"].ToString();
			} catch {
				teleOpStrategyEditor.Text = "<No Data Recorded>";
			}

			// Additional Notes Editor
			Label notesLabel = new Label {
				Text = "Additional Notes",
				FontSize = GlobalVariables.sizeMedium,
			};

			Editor notesEditor = new Editor(){
				HeightRequest = 200,
				BackgroundColor = Color.Gray
			};
			try{
				if(teamData["notes"] != null)
					notesEditor.Text = data ["notes"].ToString();
			} catch {
				notesEditor.Text = "<No Data Recorded>";
			}

			data = teamData;

			Button updateBtn = new Button () {
				Text = "Update Button",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			updateBtn.Clicked += (object sender, EventArgs e) => {
				// UpdateBtn
				errorHandling("driveType", driveTypePicker.Title);
				errorHandling("lowBarAccess", lowBarPicker.Title);
				errorHandling("autoStrategy", autoStrategyEditor.Text.ToString());
				errorHandling("teleOpStrategy", teleOpStrategyEditor.Text.ToString());
				errorHandling("notes", notesEditor.Text.ToString());

				// DisplayAlert if save did not go through
				if(error == true){
					errorString.Remove(errorString.Length - 2);
					DisplayAlert("Error", errorString, "OK");
					errorString = errorStringDefault;
					error = false;
				} else{
					Navigation.PopModalAsync();
				}
			};

			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new PitScoutingEditPage(data));
			};

			//Back Button
			Button backBtn = new Button () {
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			StackLayout side = new StackLayout () {
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,

				Children = {
					teamNumber,
					teamName
				}
			};
			StackLayout info = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,


				Children = {
					driveTypeLabel,
					driveTypePicker,
					lowBarLabel,
					lowBarPicker,
					autoStrategyLabel,
					autoStrategyEditor,
					teleOpStrategyLabel,
					teleOpStrategyEditor,
					notesLabel,
					notesEditor
				}

			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,

				Children = {
					backBtn,
					refreshBtn,
					updateBtn
				}
			};

			grid.Children.Add (robotImage, 0, 0);
			grid.Children.Add (side, 1, 0);
			grid.Children.Add (info, 0, 2, 1, 2);
			grid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content=grid
			};
		}

		void errorHandling(string d, string i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, int i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, bool i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				error = true;
				errorString += d + " , ";
			}
		}

		async void SaveData(){
			await data.SaveAsync ();
		}

		public byte[] ImageToBinary(string imagePath)
		{
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}

		async Task OpenImagePicker(){
			//It works? Don't use gallery
			var robotImagePicker = new MediaPicker(Forms.Context);

			await robotImagePicker.TakePhotoAsync(new StoreCameraMediaOptions {
				Name = data["teamNumber"].ToString() + ".jpg",
				Directory = "Robot Images"
			}).ContinueWith(t=>{
				robotImageFile = t.Result;
				Console.WriteLine("Robot Image Path: " + robotImageFile.Path);
			},TaskScheduler.FromCurrentSynchronizationContext());

			robotImage.Source = robotImageFile.Path;
			try{
				ParseFile image = new ParseFile(data["teamNumber"].ToString()+".jpg", ImageToBinary(robotImageFile.Path));

				data["robotImage"] = image;
				await data.SaveAsync();
			}
			catch{
				Console.WriteLine ("Image Save Error");
			}
		}

		void addRobotImage(){
			var imageTap = new TapGestureRecognizer ();
			imageTap.Tapped += (s, e) => {
				Console.WriteLine ("Tapped");
				OpenImagePicker ();
			};

			// RobotImage
			try {
				if (data ["robotImage"].ToString () != null) {
					ParseFile robotImageURL = (ParseFile)data ["robotImage"];
					// Gets the image from parse and converts it to ParseFile
					// robotImage.Source = "I"+teamData["teamNumber"]+".jpg"; //Must scale down images manually before upload, & all images must be .jpg
					// How to write this so caching actually works?

					robotImage.Source = new UriImageSource {
						Uri = robotImageURL.Url,
						CachingEnabled = true,
						CacheValidity = new TimeSpan (7, 0, 0, 0) // Caches Images onto your device for a week
					};
				} else {
				}
			} catch {
				robotImage.Source = "Placeholder_image_placeholder.png";
				robotImage.GestureRecognizers.Add (imageTap);
			}
			robotImage.Aspect = Aspect.AspectFit; //Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
		}
	}
}

