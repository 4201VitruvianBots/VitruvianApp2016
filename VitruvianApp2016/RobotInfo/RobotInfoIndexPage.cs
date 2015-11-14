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

		public RobotInfoIndexPage ()
		{
			//Page Title
			Label title = new Label () {
				Text = "Robot Information",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateTeamList();
			};

			//Back Button
			Button backBtn = new Button (){
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
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
					teamList,
					navigationBtns
				}
			};
		}
		async Task UpdateTeamList(){
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
					Navigation.PushModalAsync (new RobotInfoViewPage (obj));
				};
				cell.GestureRecognizers.Add (tap);
			}
		}
	}
}

