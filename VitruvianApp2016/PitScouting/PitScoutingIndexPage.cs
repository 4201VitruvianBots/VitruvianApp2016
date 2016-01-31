using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class PitScoutingIndexPage:ContentPage
	{
		StackLayout teamStack = new StackLayout();

		ActivityIndicator busyIcon = new ActivityIndicator ();

		public PitScoutingIndexPage ()
		{
			//Page Title
			Label title = new Label () {
				Text = "Pit Scouting",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateTeamList();
			};

			//Back Button
			Button backBtn = new Button (){
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
				UpdateTeamList();
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
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
		}
		async Task UpdateTeamList(){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");

			var allTeams = await sorted.FindAsync();
			teamStack.Children.Clear();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchAsync ();
				TeamListCell cell = new TeamListCell ();
				cell.teamName.Text = "Team " + obj ["teamNumber"];
				teamStack.Children.Add (cell);
				TapGestureRecognizer tap = new TapGestureRecognizer ();
				tap.Tapped += (object sender, EventArgs e) => {
					Navigation.PushModalAsync (new PitScoutingEditPage (obj));
				};
				cell.GestureRecognizers.Add (tap);
			}
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}