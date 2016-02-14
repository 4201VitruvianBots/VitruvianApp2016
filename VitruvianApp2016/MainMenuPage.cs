using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class MainMenuPage : ContentPage
	{
		public MainMenuPage ()
		{
			//Title
			Label title = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Main Menu",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			//Robot Info Tab Navigation
			Button infoBtn = new Button () {
				Text = "Robot Information"
			};
			buttonSettings (infoBtn);
			infoBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new RobotInfoIndexPage ());
			};

			//Pit Scouting Navigation
			Button pitBtn = new Button () {
				Text = "Pit Scouting"
			};
			buttonSettings (pitBtn);
			pitBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new PitScoutingIndexPage ());
			};

			//Match Scouting Tab Navigation
			Button matchBtn = new Button (){
				Text = "Match Scouting"
			};
			buttonSettings (matchBtn);
			matchBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new PreMatchScoutingPage ());
			};

			//Match Data
			Button matchDataBtn = new Button (){
				Text = "Match Data"
			};
			buttonSettings (matchDataBtn);
			matchDataBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new MatchDataSearchPage());
			};

			//Match Overview
			Button overviewBtn = new Button (){
				Text = "Match Overview",
			};
			buttonSettings (overviewBtn);
			overviewBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new MatchInfoSearchPage());
			};



			//Test Page Button
			Button testBtn = new Button (){
				Text = "Test Button"
			};
			buttonSettings (testBtn);
			testBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new NullTest());
			};
			//Page Layout
			StackLayout pageStack = new StackLayout (){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 20,

				Children = {
					title,
					infoBtn,
					pitBtn,
					matchBtn,
					matchDataBtn,
					overviewBtn,
					testBtn
				}
			};

			this.Content = new ScrollView(){
				Content = pageStack
			};
			BackgroundColor = Color.White;

		}

		void buttonSettings(Button btn){
			btn.BackgroundColor = Color.Green;
			btn.FontAttributes = FontAttributes.Bold;
			btn.TextColor = Color.White;
			btn.FontSize = GlobalVariables.sizeMedium;
		}
	}
}

