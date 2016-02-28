using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class RobotInfoIndexPage : ContentPage
	{
		StackLayout teamStack = new StackLayout();

		ActivityIndicator busyIcon = new ActivityIndicator ();
		public RobotInfoIndexPage ()
		{
			//Page Title
			Label title = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Robot Information",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus())
					UpdateTeamList();
			};

			//Back Button
			Button backBtn = new Button (){
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			ScrollView teamList = new ScrollView ();
			teamList.Content = teamStack;
			this.Appearing += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus())
					UpdateTeamList();
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn,
					refreshBtn
				}
			};

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					title,
					busyIcon,
					teamList,
					navigationBtns
				}
			};
			BackgroundColor = Color.White;
		}
		async void UpdateTeamList(){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");

			var allTeams = await sorted.FindAsync();
			teamStack.Children.Clear();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchIfNeededAsync ();
				TeamListCell cell = new TeamListCell ();
				cell.teamName.Text = "Team " + obj ["teamNumber"];
				teamStack.Children.Add (cell);
				TapGestureRecognizer tap = new TapGestureRecognizer ();
				tap.Tapped += (object sender, EventArgs e) => {
					Navigation.PushModalAsync (new RobotInfoViewPage (obj));
				};
				cell.GestureRecognizers.Add (tap);
			}
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

