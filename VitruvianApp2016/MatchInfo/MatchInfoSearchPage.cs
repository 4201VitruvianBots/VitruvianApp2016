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
		ParseObject selectMatch;

		StackLayout dataList = new StackLayout();

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
				Text = "Match Overview Search",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle
			};

			Label teamSearch = new Label () {
				Text = "Search by Team",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamSearchEntry = new Entry () {
				Placeholder = "[To be Added]",  // CUrrently Broken
				Keyboard = Keyboard.Numeric
			};

			Button teamSearchBtn = new Button () {
				Text = "Lookup Team",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			teamSearchBtn.Clicked += (object sender, EventArgs e) => {
				DisplayAlert("Error:", "Functionality not added", "OK");
				try{
					//filterMatches(1, Convert.ToInt32(teamSearchEntry.Text));
				} catch{
					//DisplayAlert("Error:", "Invalid search query", "OK");
				}
			};

			Label matchSearch = new Label () {
				Text = "Search by Match",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry matchSearchEntry = new Entry () {
				Placeholder = "[Enter a match Number]",
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
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			// Navigation Panel
			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					backBtn
				}
			};

			layoutGrid.Children.Add (title, 0, 2, 0, 1);
			layoutGrid.Children.Add (teamSearch, 0, 1, 1, 2);
			layoutGrid.Children.Add (teamSearchEntry, 0, 1, 2, 3);
			layoutGrid.Children.Add (teamSearchBtn, 1, 2, 2, 3);
			layoutGrid.Children.Add (matchSearch, 0, 1, 3, 4);
			layoutGrid.Children.Add (matchSearchEntry, 0, 1, 4, 5);
			layoutGrid.Children.Add (matchSearchBtn, 1, 2, 4, 5);
			layoutGrid.Children.Add (allMatchesBtn, 1, 2, 5, 6);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 7, 8);

			this.Content = new ScrollView () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async void filterMatches(int searchType, int number){
			
			ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchList");
			ParseQuery<ParseObject> sorted = query.OrderBy("matchNo");
			ParseQuery<ParseObject> filter;
			ParseObject selectMatch = null;
			IEnumerable<ParseObject> dataSelect = null;

			if (searchType == 1) {
				//Search multiple columns except matchNo for a teamNo ?
				filter = sorted.WhereEqualTo ("red", number);			
				dataSelect = await filter.FindAsync();
				try{
					await Navigation.PushModalAsync(new MatchInfoIndexPage(dataSelect));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			} else if (searchType == 2) {
				filter = query.WhereEqualTo ("matchNo", number);
				dataSelect = await filter.FindAsync();
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
		}
	}
}

