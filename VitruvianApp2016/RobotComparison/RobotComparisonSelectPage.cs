using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class RobotComparisonSelectPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		StackLayout dataList = new StackLayout();

		int spanYi=0;
		int spanYf=1;

		public RobotComparisonSelectPage ()
		{
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;

			Grid layoutGrid = new Grid () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto }, 
					new RowDefinition{ Height = GridLength.Auto }, 
					new RowDefinition{ Height = GridLength.Auto }, 
					new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition{ Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition{ Width = GridLength.Auto },
				}
			};

			Label title = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Robot Comparison",
				TextColor = Color.White,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label teamSearch1 = new Label () {
				Text = "Search Team 1",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamSearchEntry1 = new Entry () {
				Placeholder = "[Enter a Team Number]",
				Keyboard = Keyboard.Numeric
			};

			Picker teamPicker1 = new Picker(){ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Title = "Select Team 1"			
			};
			populateTeamPicker(teamPicker1);
			teamPicker1.SelectedIndexChanged += (sender, e) => {
				teamSearchEntry1.Text = teamPicker1.Items[teamPicker1.SelectedIndex];
			};

			Label teamSearch2 = new Label () {
				Text = "Search Team 2",
				TextColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamSearchEntry2 = new Entry () {
				Placeholder = "[Enter a Team Number]",
				Keyboard = Keyboard.Numeric
			};

			Picker teamPicker2 = new Picker(){ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Title = "Select Team 2"			
			};
			populateTeamPicker(teamPicker2);
			teamPicker2.SelectedIndexChanged += (sender, e) => {
				teamSearchEntry2.Text = teamPicker2.Items[teamPicker2.SelectedIndex];
			};

			Button teamSearchBtn = new Button () {
				Text = "Compare Teams",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			teamSearchBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus()){
					if(string.IsNullOrEmpty(teamSearchEntry1.Text) || string.IsNullOrEmpty(teamSearchEntry2.Text))
						DisplayAlert("Error", "Enter Team Numbers", "OK");
					else{
						try{
							filterMatches(Convert.ToInt32(teamSearchEntry1.Text), Convert.ToInt32(teamSearchEntry2.Text));
						} catch{
							DisplayAlert("Error:", "Invalid search query", "OK");
						}
					}
				}
			};

			//Back Button
			Button backBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			// Navigation Panel
			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn
				}
			};

			layoutGrid.Children.Add (teamSearch1, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (teamSearch2, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamSearchEntry1, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (teamSearchEntry2, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamPicker1, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (teamPicker2, 1, 2, spanYi, spanYf);
			layoutGrid.Children.Add (teamSearchBtn, 3, 4, spanYi++, spanYf++);

			StackLayout pageStack = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					title,
					busyIcon,
					layoutGrid,
					navigationBtns
				}
			};

			this.Content = new ScrollView () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = pageStack
			};
			BackgroundColor = Color.Gray;
		}

		async void filterMatches(int teamNumber1, int teamNumber2){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");
			ParseQuery<ParseObject>[] filter = new ParseQuery<ParseObject>[2];
			ParseObject team1 = null;
			ParseObject team2 = null;
			IEnumerable<ParseObject> dataSelect1 = null;
			IEnumerable<ParseObject> dataSelect2 = null;

			try{
				filter[0] = query.WhereEqualTo ("teamNumber", teamNumber1);
				dataSelect1 = await filter[0].FindAsync();
				foreach (ParseObject obj in dataSelect1) {
					team1 = obj;
				}
				filter[1] = query.WhereEqualTo ("teamNumber", teamNumber2);
				dataSelect2 = await filter[1].FindAsync();
				foreach (ParseObject obj in dataSelect2) {
					team2 = obj;
				}
				try{
					if (new CheckInternetConnectivity().InternetStatus())
						await Navigation.PushModalAsync(new RobotComparisonDisplayPage(team1, team2));
				}
				catch{
					DisplayAlert("Error:", "Couldn't pull teams", "OK");
				}
			}
			catch{
				DisplayAlert("Error:", "Invalid Team Number(s)", "OK");
			}


			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		async void populateTeamPicker(Picker pick){
			if (new CheckInternetConnectivity ().InternetStatus ()) {
				ParseQuery<ParseObject> query = ParseObject.GetQuery ("TeamData");
				ParseQuery<ParseObject> sorted = query.OrderBy ("teamNumber");

				var allTeams = await sorted.FindAsync ();
				pick.Items.Clear ();
				foreach (ParseObject obj in allTeams) {
					pick.Items.Add (obj ["teamNumber"].ToString ());
				}
			}
		}
	}
}

