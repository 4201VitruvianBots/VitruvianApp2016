using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class PitScoutingEditPage:ContentPage

	{
		ParseObject data;
			
		enum Choice{Yes, No};
		enum DriveType {Swerve, Mechanum, WestCoast, Omni, OtherHolonomic, Other};
		enum Orientation {Widthwise, Lengthwise};
		enum Position{RightSideUp, TippedOver};
		enum Number{Zero, One, Two, Three};

		bool error = false;

		string errorString = "The Following Data Was Unable To Be Saved: ";

		public PitScoutingEditPage (ParseObject teamData)
		{                                                                      //Entry any value
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

			Label teamNumber = new Label ();
			try {
				if (teamData ["teamNumber"] != null) {
					teamNumber.Text = teamData ["teamNumber"].ToString();
				} else {}
			}
			catch {
				teamNumber.Text = "<No Team Number>";
			}
			teamNumber.FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label));

			Label teamName = new Label ();
			try {
				if (teamData ["teamName"] != null) {
					teamName.Text = teamData ["teamName"].ToString();
				} else {} 
			} catch {
				teamName.Text = "<No Team Name>";
			}
			teamName.FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label));

			Label robotWeightLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Robot Weight",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};

			Entry robotWeightEntry = new Entry () {
				Keyboard = Keyboard.Numeric
			};
			try{
				if (teamData ["robotWeight"] != null)
					robotWeightEntry.Text = teamData ["robotWeight"].ToString();
			} catch {
				robotWeightEntry.Placeholder = "Enter Robot Weight";
			}

			Label rampChoiceLabel = new Label () { 
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Ramp Choice",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label)) //Properities and Such
		
			};                                                           //Picker Yes and No
			var rampChoice = new Picker ();
			rampChoice.Title = "Pick a Choice";
			for (Choice i = Choice.No; i <= Choice.Yes; i++) {
				rampChoice.Items.Add (i.ToString ());
			}
			;
			rampChoice.SelectedIndexChanged += (sender, e) => {
				rampChoice.Title = rampChoice.SelectedIndex.ToString ();
			};

			Label driveTypeLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Drive Type",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			
			};
		
			var driveType = new Picker ();
			driveType.Title = "Drive Type";
			for (DriveType i = DriveType.Swerve; i <= DriveType.Other; i++) {
				driveType.Items.Add (i.ToString ());
			}
			;
			driveType.SelectedIndexChanged += (sender, e) => {
				driveType.Title = driveType.SelectedIndex.ToString ();
			};

			Label toteOrientationLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Tote Orientation",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			
			};
			var toteOrientation = new Picker ();
			toteOrientation.Title = "Tote Pickup Orientation";
			for (Orientation i = Orientation.Widthwise; i <= Orientation.Lengthwise; i++) {
				toteOrientation.Items.Add (i.ToString ());
			}
			;
			toteOrientation.SelectedIndexChanged += (sender, e) => {
				toteOrientation.Title = toteOrientation.SelectedIndex.ToString ();
			};

			Label coopertitionTotesLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Coopertition Totes",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};
			var coopertitionTotes = new Picker ();
			coopertitionTotes.Title = "# Of Coopertition Totes They Can Stack";
			for (Number i = Number.Zero; i <= Number.Three; i++) {
				coopertitionTotes.Items.Add (i.ToString ());
			}
			;
			coopertitionTotes.SelectedIndexChanged += (sender, e) => {
				coopertitionTotes.Title = coopertitionTotes.SelectedIndex.ToString ();
			
			};
			Label autoToteLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Auto Tote",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};
			var autoTote = new Picker ();
			autoTote.Title = "Can I Push Tote In Auto?";
			for (Choice i = Choice.Yes; i <= Choice.No; i++) {
				autoTote.Items.Add (i.ToString ());
			}
			;
			autoTote.SelectedIndexChanged += (sender, e) => {
				autoTote.Title = autoTote.SelectedIndex.ToString ();
			};

			Label canOrientationLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Can Orientation",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};
			var canOrientation = new Picker ();
			canOrientation.Title = "Can Pickup Orientation";
			for (Position i = Position.RightSideUp; i <= Position.TippedOver; i++) {
				canOrientation.Items.Add (i.ToString ());
			}
			;
			canOrientation.SelectedIndexChanged += (sender, e) => {
				canOrientation.Title = canOrientation.SelectedIndex.ToString ();
			};
			Label autoStrategyLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Auto Strategy",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};

			Editor autoStrategyEditor = new Editor () {
				//Text = "Auto Strategy",
				HeightRequest = 200,
				BackgroundColor = Color.Gray
			};
			
			Label teleOpStrategyLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				Text = "TeleOp Strategy",
				FontSize = Device.GetNamedSize (NamedSize.Small, typeof(Label))
			};

			Editor teleOpStrategyEditor = new Editor () {
				//Text = "TeleOp Strategy",
				HeightRequest = 200,
				BackgroundColor =Color.Gray
			};	
			//Editor (For when you want notes for pit scouting. Un-Quantifiable)

			Label additionalNotesLabel = new Label () {
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				//Text = "Additional Notes",
				FontSize = GlobalVariables.sizeSmall
			};

			Editor additionalNotesEditor = new Editor () {
				//Text = "Additional Notes",
				HeightRequest = 200,
				BackgroundColor = Color.Gray
			
				
			};
			data = teamData;
			Button updateButton = new Button () {
				Text = "Update Button" 
			};
			updateButton.Clicked += (object sender, EventArgs e) => {
				//start (copy)
				try{ 
					if(robotWeightEntry.Text != "Enter Robot Weight"){
						data["robotWeight"] = Convert.ToInt32(robotWeightEntry.Text);
						SaveData ();
					}
				

				}catch {
					error = true;
					errorString += "robotWeight, "; 	
				}
				//One
				try{ 
					if(rampChoice.Title != "Pick a Choice"){
						data["rampChoice"] = rampChoice.Title;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "rampChoice, "; 
				}
				//Second
				try{ 
					if(driveType.Title != "Drive Type"){
						data["driveType"] = driveType.Title;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "driveType, "; 
				}//Third
				try{ 
					if(toteOrientation.Title != "Tote Pickup Orientation"){
						data["toteOrientation"] = toteOrientation.Title;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "toteOrientation, "; 
				}//Fourth
				try{ 
					if(coopertitionTotes.Title != "# of Coopertition Totes They Can Stack"){
						data["coopertitionTotes"] = Convert.ToInt32(coopertitionTotes.Title);
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "coopertitionTotes, "; 
				}

				//Fifth

				try{ 
					if(autoTote.Title != "Can I Push Totes In Auto?"){
						data["autoTote"] = autoTote.Title;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "autoTote, "; 
				}
				//sixth
				try{ 
					if(canOrientation.Title != "Can Pickup Orientation"){
						data["canOrientation"] = canOrientation.Title;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "canOrientation, "; 
				}
				//seventh
				try{ 
					if(autoStrategyEditor.Text != "Auto Strategy"){
						data["autoStrategy"] = autoStrategyEditor.Text;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "autoStrategy, "; 
				}
				//eighth
				try{ 
					if(teleOpStrategyEditor.Text != "TeleOp Strategy"){
						data["teleOpStrategy"] = teleOpStrategyEditor.Text;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "teleOpStrategy, "; 
				}
				//ninth
				try{ 
					if(additionalNotesEditor.Text != "Additional Notes"){
						data["additionalNotes"] = additionalNotesEditor.Text;
						SaveData ();
					}


				}catch {
					error = true;
					errorString += "additionalNotes, "; 
				}
				//End
				if(error == true){
					errorString = errorString.Remove(errorString.Length - 2);
					DisplayAlert("Error:", errorString, "OK");
				} else {
					Navigation.PopModalAsync ();
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
					robotWeightLabel,
					robotWeightEntry,
					toteOrientationLabel,
					toteOrientation,
					canOrientationLabel,
					canOrientation,
					autoStrategyLabel,
					autoStrategyEditor,
					autoToteLabel,
					autoTote,
					teleOpStrategyLabel,
					teleOpStrategyEditor,
					coopertitionTotesLabel,
					coopertitionTotes,
					additionalNotesLabel,
					additionalNotesEditor

					//all entities on view team page


				}

			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,

				Children = {
					backBtn,
					refreshBtn,
					updateButton
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

