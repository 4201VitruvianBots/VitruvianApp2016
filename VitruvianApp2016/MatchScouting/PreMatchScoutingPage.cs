using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class PreMatchScoutingPage:ContentPage
	{
		ParseObject MatchData = new ParseObject("MatchData");

		enum Alliance {red1, red2, red3, blue1, blue2, blue3};
		enum defenses {Portcullis, Cheval_de_Frise, Moat, Ramparts, Drawbridge, Salley_Port, Rock_Wall, Rough_Terrain};
		enum defA {Portcullis, Cheval_de_Frise};
		enum defB {Moat, Ramparts};
		enum defC {Drawbridge, Salley_Port};
		enum defD {Rock_Wall, Rough_Terrain};
		Picker[] defPicker = new Picker[4]{new Picker(),new Picker(),new Picker(),new Picker()};
		Label[] defLabel = new Label[4]{ new Label (), new Label (), new Label (), new Label () };
		int[] def = new int[4] { -1, -1, -1, -1};

		public PreMatchScoutingPage (string name)
		{
			Label matchScoutingLabel = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Match Scouting",
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold,
				BackgroundColor = Color.Black,
			};

			Label scouterNameLabel = new Label {
				Text = "Scouter Name:",
				TextColor = Color.Black,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry scouterNameEntry = new Entry {
			};
			if (name == null)
				scouterNameEntry.Placeholder = "[Enter your name]";
			else
				scouterNameEntry.Text = name;

			Label matchNoLabel = new Label {
				Text = "Match Number:",
				TextColor = Color.Black,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry matchNo = new Entry {
				Placeholder = "[Enter Match No.]"
			};
			matchNo.Keyboard = Keyboard.Numeric;

			Label teamNoLabel = new Label {
				Text = "Team Number:",
				TextColor = Color.Black,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry teamNo = new Entry {
				Placeholder = "[Enter Team No.]"
			};
			teamNo.Keyboard = Keyboard.Numeric;

			// AlliancePicker
			Label allianceLabel = new Label {
				Text = "Alliance Position:",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker alliancePicker = new Picker();
			alliancePicker.Title = "Choose an Option";
			for (Alliance i = Alliance.red1; i <= Alliance.blue3; i++) {
				alliancePicker.Items.Add (i.ToString ());
			};
			alliancePicker.SelectedIndexChanged += (sender, e) => {
				Alliance ally = (Alliance)alliancePicker.SelectedIndex;
				alliancePicker.Title = ally.ToString();
			};

			// positionPicker
			Label positionLabel = new Label {
				Text = "Starting Position:",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker positionPicker = new Picker();
			positionPicker.Title = "Choose an Option";
			for (int i = 1; i <= 7; i++) {
				positionPicker.Items.Add (i.ToString ());
			};
			positionPicker.SelectedIndexChanged += (sender, e) => {
				positionPicker.Title = positionPicker.SelectedIndex.ToString ();
			};

			// defPickers
			for (int i = 0; i < 8; i++) {
				defPicker [0].Items.Add (((defenses)i).ToString ());
				defPicker [1].Items.Add (((defenses)i).ToString ());
				defPicker [2].Items.Add (((defenses)i).ToString ());
				defPicker [3].Items.Add (((defenses)i).ToString ());
			}
			defFunction ("Position 2:", 0);
			defFunction ("Position 3:", 1);
			defFunction ("Position 4:", 2);
			defFunction ("Position 5:", 3);

			//Start Match Scout
			Button beginScoutBtn = new Button {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Begin Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			beginScoutBtn.Clicked += (object sender, EventArgs e) => {
				def[0]=defPicker[0].SelectedIndex;
				def[1]=defPicker[1].SelectedIndex;
				def[2]=defPicker[2].SelectedIndex;
				def[3]=defPicker[3].SelectedIndex;

				if (new CheckInternetConnectivity().InternetStatus()){
					if(string.IsNullOrEmpty(scouterNameEntry.Text)|| string.IsNullOrEmpty(matchNo.Text) || string.IsNullOrEmpty(teamNo.Text) ||positionPicker.Title.ToString() == "Choose an Option" || defPicker[0].Title.ToString() == "Choose an Option" || defPicker[1].Title.ToString() == "Choose an Option" || defPicker[2].Title.ToString() == "Choose an Option" || defPicker[3].Title.ToString() == "Choose an Option"){
						DisplayAlert("Error", "Fill out all Fields", "Ok");
					} 
					else if(def[0] == def[1] || def[0] == def[2] || def[0] == def[3] || def[1] == def[2] || def[1] == def[3] || def[2] == def[3]){
						DisplayAlert("Error", "You cannot have multiples of a single defense", "Ok");
					} else {
						Console.WriteLine(matchNo.Text);
						MatchData["scouterName"]=scouterNameEntry.Text;
						MatchData["teamNo"]=Convert.ToInt32(teamNo.Text);
						MatchData["matchNo"]=Convert.ToInt32(matchNo.Text);
						MatchData["alliance"]=Convert.ToString(alliancePicker.Title.ToString());
						MatchData["startPosition"]=Convert.ToInt32(positionPicker.Title.ToString());
						MatchData["pos2"]=((defenses)def[0]).ToString();
						MatchData["pos3"]=((defenses)def[1]).ToString();
						MatchData["pos4"]=((defenses)def[2]).ToString();
						MatchData["pos5"]=((defenses)def[3]).ToString();

						/*
						MatchData.Add("teamNo", Convert.ToInt32(teamNo.Text));
						MatchData.Add("matchNo", Convert.ToInt32(matchNo.Text));
						MatchData.Add("Alliance", Convert.ToString(alliancePicker.Title.ToString()));
						MatchData.Add("startPosition", Convert.ToInt32(positionPicker.Title.ToString()));
						*/
						SaveData();

						Navigation.PushModalAsync(new AutoMatchScoutingPage(MatchData, def, Convert.ToInt16(teamNo.Text)));
					}
				}
			};

			//Back Button
			Button backBtn = new Button (){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new MainMenuPage()); // This must be push because stack of pages will be different after a full match scout
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn,
					beginScoutBtn
				}
			};

			StackLayout pageLayout = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					matchScoutingLabel,
					scouterNameLabel,
					scouterNameEntry,
					matchNoLabel,
					matchNo,
					teamNoLabel,
					teamNo,
					allianceLabel,
					alliancePicker,
					positionLabel,
					positionPicker,
				}
			};
			for(int i=0; i<4; i++){
				pageLayout.Children.Add(defLabel[i]);
				pageLayout.Children.Add(defPicker[i]);
			}
			pageLayout.Children.Add(navigationBtns);

			this.Content = new ScrollView(){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content=pageLayout
			};
			BackgroundColor = Color.Gray;
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await MatchData.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}

		void defFunction(string title, int arrayIndex){
			defLabel [arrayIndex].Text = title;
			defLabel [arrayIndex].TextColor = Color.Black;
			defLabel [arrayIndex].FontSize = GlobalVariables.sizeMedium;

			defPicker [arrayIndex].Title = "Choose an Option";
			defPicker[arrayIndex].SelectedIndexChanged += (sender, e) => {
				defPicker[arrayIndex].Title = defPicker[arrayIndex].SelectedIndex.ToString ();
			};
		}
	}
}
