using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class CalculateAverageDataPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator();
		Grid pageGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition{ Height = GridLength.Auto },
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto },
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
				new ColumnDefinition{ Width = GridLength.Auto },
			}
		};
		StackLayout consoleStack = new StackLayout();
		int i=0;
		Label[] teamLabel= new Label[99];
		public CalculateAverageDataPage ()
		{
			Label pageTitle = new Label () {
				Text = "Calculate Team Data",
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				FontAttributes = FontAttributes.Bold,
				FontSize = GlobalVariables.sizeTitle
			};

			Label teamNumberLabel = new Label () {
				Text = "Enter Team Number:",
				FontAttributes = FontAttributes.Bold,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamNumberEntry = new Entry () {
				Placeholder = "[Enter Team No.]",
				Keyboard = Keyboard.Numeric
			};

			Button calculateTeamBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Calculate Team",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			calculateTeamBtn.Clicked += (object sender, EventArgs e) => {
				try{
					allTeams(Convert.ToInt16(teamNumberEntry.Text), 1);
				}
				catch{
					DisplayAlert("Error", "Enter Team No.", "OK");
				}
			};

			Button calculateAllTeamsBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Calculate All Teams",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			calculateAllTeamsBtn.Clicked += (object sender, EventArgs e) => {
				try{
					allTeams(0, 0);
				}
				catch{
					DisplayAlert("Error", "Error", "OK");
				}
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

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn
				}
			};
			ScrollView teamScroll = new ScrollView(){
				Content = consoleStack
			};
			pageGrid.Children.Add (teamScroll, 0, 3 ,4,5);
			pageGrid.Children.Add (pageTitle,0,0);
			pageGrid.Children.Add (busyIcon,1,0);
			pageGrid.Children.Add (teamNumberLabel,0,1);
			pageGrid.Children.Add (teamNumberEntry,0,2);
			pageGrid.Children.Add (calculateTeamBtn,2,2);
			pageGrid.Children.Add (calculateAllTeamsBtn,2,3);
			pageGrid.Children.Add (navigationBtns,0, 3, 5,6);
			this.Content = new StackLayout {
				Children = {
					pageGrid
				}
			};
		}

		async void allTeams(int teamNo, int type){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");

			consoleStack.Children.Clear();
			if (type == 0) {
				var allTeams = await sorted.FindAsync();
				foreach (ParseObject obj in allTeams) {
					await obj.FetchAsync ();
					teamLabel [i] = new Label () {
						Text = "calculating " + obj ["teamNumber"].ToString ()
					};
					new CalculateAverageData (Convert.ToInt16 (obj ["teamNumber"]));
					consoleStack.Children.Add (teamLabel [i]);
					i++;
				}
			} else if (type == 1) {
				ParseQuery<ParseObject> filter = query.WhereEqualTo("teamNumber", teamNo);
				var allTeams = await filter.FindAsync();
				foreach (ParseObject obj in allTeams) {
					await obj.FetchAsync ();
					teamLabel [i] = new Label () {
						Text = "calculating " + obj ["teamNumber"].ToString () +"..."
					};
					new CalculateAverageData (Convert.ToInt16 (obj ["teamNumber"]));
					consoleStack.Children.Add (teamLabel [i]);
					i++;
				}
			}
			teamLabel[i] = new Label(){
				Text = "Done"
			};

			consoleStack.Children.Add (teamLabel[i]);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

