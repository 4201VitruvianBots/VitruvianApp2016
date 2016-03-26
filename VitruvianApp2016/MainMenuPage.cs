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
				Text = "Team 4201 Scouting App",
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
				Navigation.PushModalAsync (new PreMatchScoutingPage (null));
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

			//Robot Comparison
			Button robotComparisonBtn = new Button (){
				Text = "Robot Comparison",
			};
			buttonSettings (robotComparisonBtn);
			robotComparisonBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new RobotComparisonSelectPage());
			};

			//Team Comparison
			Button teamComparisonBtn = new Button (){
				Text = "Team Comparison",
			};
			buttonSettings (teamComparisonBtn);
			teamComparisonBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new StatComparsionPage());
			};

			// Cloud Calculation
			Button cloudBtn = new Button (){
				Text = "Calculate All Team Data",
			};
			buttonSettings (cloudBtn);
			cloudBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new CalculateAverageDataPage());
			};
				
			//Test Page Button
			Button testBtn = new Button (){
				Text = "Test Button"
			};
			buttonSettings (testBtn);
			testBtn.Clicked += (object sender, EventArgs e) => {
				//Navigation.PushModalAsync (new RobotComparisonDisplayPage());
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
					robotComparisonBtn,
					teamComparisonBtn,
					cloudBtn,
					//testBtn
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

