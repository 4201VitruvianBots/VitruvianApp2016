using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class PitScoutEditPage2:ContentPage
	{
		ParseObject data;

		enum DriveTypes {Tank, WestCoast, Holonomic, Omni, Mechanum, Other};
		enum Choice {Yes, No};

		const string errorStringDefault = "The Following Data Was Unable To Be Saved: ";
		string errorString = errorStringDefault;
		bool error = false;

		Image robotImage = new Image();

		public PitScoutEditPage2 (ParseObject teamData)
		{
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

			// RobotImage
			try {
				if (teamData ["robotImage"].ToString() != null) {
					ParseFile robotImageURL = (ParseFile)teamData ["robotImage"];
					// Gets the image from parse and converts it to ParseFile
					// robotImage.Source = "I"+teamData["teamNumber"]+".jpg"; //Must scale down images manually before upload, & all images must be .jpg
					// How to write this so caching actually works?

					robotImage.Source = new UriImageSource{
						Uri = robotImageURL.Url,
						CachingEnabled = true,
						CacheValidity = new TimeSpan(7,0,0,0) // Caches Images onto your device for a week
					};
				} else {}
			}
			catch {
				robotImage.Source = "Placeholder_image_placeholder.png";
			}
			robotImage.Aspect = Aspect.AspectFit; //Need better way to scale an image while keeping aspect ratio, but not overflowing everything else
			// robotImage.GestureRecognizers.Add (imageTap);

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
			driveTypePicker.Title = "Choose an Option";
			for (DriveTypes i = DriveTypes.Tank; i <= DriveTypes.Other; i++) {
				driveTypePicker.Items.Add (i.ToString ());
			}
			;
			driveTypePicker.SelectedIndexChanged += (sender, e) => {
				driveTypePicker.Title = driveTypePicker.SelectedIndex.ToString ();
			};

			// Low Bar Picker
			Label lowBarLabel = new Label {
				Text = "Can you go under the low bar?:",
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker lowBarPicker = new Picker();
			lowBarPicker.Title = "Choose an Option";
			for (Choice i = Choice.Yes; i <= Choice.No; i++) {
				lowBarPicker.Items.Add (i.ToString ());
			}
			;
			lowBarPicker.SelectedIndexChanged += (sender, e) => {
				lowBarPicker.Title = lowBarPicker.SelectedIndex.ToString ();
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
				if(data[autoStrategy] != null)
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
				if(data["teleOpStrategy"] != null)
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
				if(data["notes"] != null)
					notesEditor.Text = data ["notes"].ToString();
			} catch {
				notesEditor.Text = "<No Data Recorded>";
			}

			data = teamData;

			Button updateBtn = new Button () {
				Text = "Update Button" 
			};
			updateBtn.Clicked += (object sender, EventArgs e) => {
				// UpdateBtn
				try{
					if(driveTypePicker.Title != "Choose an Option"){
						data["driveType"] = driveTypePicker.Title.ToString();
						SaveData();
					}
				}
				catch {
					errorString += "driveType, ";
					error = true;
				}
				try{
					if(lowBarPicker.Title != "Choose an Option"){
						data["lowBarAccess"] = lowBarPicker.Title.ToString();
						SaveData();
					}
				}
				catch {
					errorString += "lowBarAccess, ";
					error = true;
				}
				try{
					if(autoStrategyEditor.Text != "<No Data Recorded>"){
						data["autoStrategy"] = autoStrategyEditor.Title.ToString();
						SaveData();
					}
				}
				catch {
					errorString += "autoStrategy, ";
					error = true;
				}
				try{
					if(teleOpStrategyEditor.Text != "<No Data Recorded>"){
						data["teleOpStrategy"] = teleOpStrategyEditor.Title.ToString();
						SaveData();
					}
				}
				catch {
					errorString += "teleOpStrategy, ";
					error = true;
				}
				try{
					if(notesEditor.Text != "<No Data Recorded>"){
						data["notes"] = notesEditor.Title.ToString();
						SaveData();
					}
				}
				catch {
					errorString += "notes, ";
					error = true;
				}

				// DisplayAlert if save did not go through
				if(error == true){
					errorString -= 2;
					DisplayAlert("Error", errorString, "OK");
					errorString = errorStringDefault;
					error = false;
				}
			};

			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new PitScoutingEditPage(teamData));
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
					driveTypeLabel,
					driveTypePicker,
					refreshBtn,
					updateBtn
				}
			};

			//grid.Children.Add (robotImage, 0, 0);
			grid.Children.Add (side, 1, 0);
			grid.Children.Add (info, 0, 2, 1, 2);
			grid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content=grid
			};
		}

		async void SaveData(){
			await data.SaveAsync ();
		}
	}
}

