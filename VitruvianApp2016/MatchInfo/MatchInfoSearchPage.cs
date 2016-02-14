using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchInfoSearchPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		StackLayout dataList = new StackLayout();

		int spanYi=0;
		int spanYf=1;

		public MatchInfoSearchPage ()
		{
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
					new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition{ Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto },
				}
			};

			Label title = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Match Overview Search",
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			Label teamSearch = new Label () {
				Text = "Search by Team",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamSearchEntry = new Entry () {
				Placeholder = "[Enter a Team Number]",  // Currently Non-functional
				Keyboard = Keyboard.Numeric
			};

			Button teamSearchBtn = new Button () {
				Text = "Lookup Team",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			teamSearchBtn.Clicked += (object sender, EventArgs e) => {
				try{
					filterMatches(1, Convert.ToInt32(teamSearchEntry.Text));
				} catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			};

			Label matchSearch = new Label () {
				Text = "Search by Match",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry matchSearchEntry = new Entry () {
				Placeholder = "[Enter a Match Number]",
				Keyboard = Keyboard.Numeric
			};

			Button matchSearchBtn = new Button () {
				Text = "Lookup Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			matchSearchBtn.Clicked += (object sender, EventArgs e) => {
				try{
					filterMatches(2, Convert.ToInt32(matchSearchEntry.Text));
				} catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			};

			Button allMatchesBtn = new Button () {
				Text = "Show all Matches",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			allMatchesBtn.Clicked += (object sender, EventArgs e) => {
				try{
					filterMatches(3, -4201);
				} catch{
					DisplayAlert("Error:", "Unknown Error", "OK");
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

			layoutGrid.Children.Add (title, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (busyIcon, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamSearch, 0, 1, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamSearchEntry, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (teamSearchBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (matchSearch, 0, 1, spanYi++, spanYf++);
			layoutGrid.Children.Add (matchSearchEntry, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (matchSearchBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (allMatchesBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (navigationBtns, 0, 2, spanYi+1, spanYf+1);

			this.Content = new ScrollView () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async void filterMatches(int searchType, int number){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			
			ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchList");
			ParseQuery<ParseObject> sorted = query.OrderBy("matchNo");
			ParseQuery<ParseObject>[] filter = new ParseQuery<ParseObject>[6];
			ParseObject selectMatch = null;
			IEnumerable<ParseObject> dataSelect = null;

			if (searchType == 1) {
				filter[0] = query.WhereEqualTo("red1", number);
				filter[1] = filter[0].Or(query.WhereEqualTo("red2", number));
				filter[2] = filter[1].Or(query.WhereEqualTo("red3", number));
				filter[3] = filter[2].Or(query.WhereEqualTo("blue1", number));
				filter[4] = filter[3].Or(query.WhereEqualTo("blue2", number));
				filter[5] = filter[4].Or(query.WhereEqualTo("blue3", number));

				dataSelect = await filter[5].FindAsync ();
				try{
					await Navigation.PushModalAsync(new MatchInfoIndexPage(dataSelect));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			} else if (searchType == 2) {
				filter[0] = query.WhereEqualTo ("matchNo", number);
				dataSelect = await filter[0].FindAsync();
				foreach (ParseObject obj in dataSelect) {
					selectMatch = obj;
				}
				try{
					await Navigation.PushModalAsync(new MatchInfoDisplayPage(selectMatch));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			} else if (searchType == 3){
				dataSelect = await sorted.FindAsync();
				await Navigation.PushModalAsync(new MatchInfoIndexPage(dataSelect));
			}
			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

