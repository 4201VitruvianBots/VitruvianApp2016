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
				TextColor = Color.Green,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Main Menu",
				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label))
			};

			//Robot Info Tab Navigation
			Button infoBtn = new Button () {
				Text = "Robot Information",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			infoBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync (new RobotInfoIndexPage ());
			};

			//Pit Scouting Navigation
			Button pitBtn = new Button () {
				Text = "Pit Scouting",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			pitBtn.Clicked += (object sender, EventArgs e) => {
				//Navigation.PushModalAsync (new PitScoutingPage ());
			};

			//Match Scouting Tab Navigation
			Button matchBtn = new Button (){
				Text = "Match Scouting",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			matchBtn.Clicked += (object sender, EventArgs e) => {
				//Navigation.PushModalAsync (new PreMatchDataPage ());
			};

			//Page Layout
			this.Content = new StackLayout (){
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Padding = 20, Spacing = 20, //new Thickness (5, 10, 5, 10); Use this to control padding or spacing on the Left, Right, Top, Bottom

				Children = {
					title,
					infoBtn,
					pitBtn,
					matchBtn
				}
			};
		}
	}
}

