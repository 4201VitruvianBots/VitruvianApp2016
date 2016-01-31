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
		enum defA {Portcullis, Cheval_de_Frise};
		enum defB {Moat, Ramparts};
		enum defC {Drawbridge, Salley_Port};
		enum defD {Rock_Wall, Rough_Terrain};
		Picker[] defPicker = new Picker[4]{new Picker(),new Picker(),new Picker(),new Picker()};
		Label[] defLabel = new Label[4]{ new Label (), new Label (), new Label (), new Label () };
		int[] def = new int[4] { -1, -1, -1, -1};

		public PreMatchScoutingPage ()
		{
			Label matchNoLabel = new Label {
				Text = "Match Number:",
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry matchNo = new Entry {
				Placeholder = "[Enter Match No.]"
			};
			matchNo.Keyboard = Keyboard.Numeric;

			Label teamNoLabel = new Label {
				Text = "Team Number:",
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry teamNo = new Entry {
				Placeholder = "[Enter Team No.]"
			};
			teamNo.Keyboard = Keyboard.Numeric;

			// AlliancePicker
			Label allianceLabel = new Label {
				Text = "Alliance Position:",
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
			for (int i = 0; i < 2; i++) {
				defPicker[0].Items.Add (((defA)i).ToString());
				defPicker[1].Items.Add (((defB)i).ToString());
				defPicker[2].Items.Add (((defC)i).ToString());
				defPicker[3].Items.Add (((defD)i).ToString());
			}
			defFunction ("Category A", 0);
			defFunction ("Category B", 1);
			defFunction ("Category C", 2);
			defFunction ("Category D", 3);

			//Start Match Scout
			Button beginScoutBtn = new Button {
				Text = "Begin Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			beginScoutBtn.Clicked += (object sender, EventArgs e) => {
				if(string.IsNullOrEmpty(matchNo.Text) || string.IsNullOrEmpty(teamNo.Text) ||positionPicker.Title.ToString() == "Choose an Option" || defPicker[0].Title.ToString() == "Choose an Option" || defPicker[1].Title.ToString() == "Choose an Option" || defPicker[2].Title.ToString() == "Choose an Option" || defPicker[3].Title.ToString() == "Choose an Option"){
					DisplayAlert("Error", "Fill out all Fields", "Ok");
				} else {
					Console.WriteLine(matchNo.Text);
					MatchData["teamNo"]=Convert.ToInt32(teamNo.Text);
					MatchData["matchNo"]=Convert.ToInt32(matchNo.Text);
					MatchData["alliance"]=Convert.ToString(alliancePicker.Title.ToString());
					MatchData["startPosition"]=Convert.ToInt32(positionPicker.Title.ToString());

					/*
					MatchData.Add("teamNo", Convert.ToInt32(teamNo.Text));
					MatchData.Add("matchNo", Convert.ToInt32(matchNo.Text));
					MatchData.Add("Alliance", Convert.ToString(alliancePicker.Title.ToString()));
					MatchData.Add("startPosition", Convert.ToInt32(positionPicker.Title.ToString()));
					*/
					SaveData();
					def[0]=defPicker[0].SelectedIndex;
					def[1]=defPicker[1].SelectedIndex;
					def[2]=defPicker[2].SelectedIndex;
					def[3]=defPicker[3].SelectedIndex;

					Navigation.PushModalAsync(new AutoMatchScoutingPage(MatchData, def));
				}
			};

			//Back Button
			Button backBtn = new Button (){
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new MainMenuPage()); // This must be push because stack of pages will be different after a full match scout
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,

				Children = {
					backBtn,
					beginScoutBtn
				}
			};

			 StackLayout pageLayout = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
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
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Fill,

				Content=pageLayout
			};
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await MatchData.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}

		void defFunction(string title, int arrayIndex){
			defLabel [arrayIndex].Text = title;
			defLabel [arrayIndex].FontSize = GlobalVariables.sizeMedium;

			defPicker [arrayIndex].Title = "Choose an Option";
			defPicker[arrayIndex].SelectedIndexChanged += (sender, e) => {
				defPicker[arrayIndex].Title = defPicker[arrayIndex].SelectedIndex.ToString ();
			};
		}
	}
}
