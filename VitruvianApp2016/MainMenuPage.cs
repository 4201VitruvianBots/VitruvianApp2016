using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class MainMenuPage : ContentPage
	{
		ParseObject obj;

		public MainMenuPage ()
		{
			//Title
			Label title = new Label () {
				TextColor = Color.Green,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Main Menu",
				FontSize = GlobalVariables.sizeTitle
			};

			//Robot Info Tab Navigation
			Button infoBtn = new Button () {
				Text = "Robot Information",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			infoBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new RobotInfoIndexPage ());
			};

			//Pit Scouting Navigation
			Button pitBtn = new Button () {
				Text = "Pit Scouting",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			pitBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new PitScoutingIndexPage ());
			};

			//Match Scouting Tab Navigation
			Button matchBtn = new Button (){
				Text = "Match Scouting",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			matchBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new PreMatchScoutingPage ());
			};

			//Test Page Button
			/*
			Button testBtn = new Button (){
				Text = "Test Button",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			testBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new TeleOpMatchScoutingPage(obj));
			};
			*/
			//Page Layout
			this.Content = new StackLayout (){
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Padding = 20, Spacing = 20, //new Thickness (5, 10, 5, 10); Use this to control padding or spacing on the Left, Right, Top, Bottom

				Children = {
					title,
					infoBtn,
					pitBtn,
					matchBtn,
					//testBtn
				}
			};
		}
	}
}

