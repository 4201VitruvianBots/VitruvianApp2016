using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchDataSearchPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		StackLayout dataList = new StackLayout();

		int spanYi=0;
		int spanYf=1;

		public MatchDataSearchPage ()
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
				Text = "Match Data Search",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle
			};

			Label teamSearch = new Label () {
				Text = "Search Team Matches",
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeMedium
			};

			Entry teamSearchEntry = new Entry () {
				Placeholder = "[Enter a team Number]",
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
				Text = "Search Match Number",
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

			Label allMatchesWarning = new Label(){
				Text = "Warning: Can be slow",
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeMedium,
				HorizontalOptions = LayoutOptions.Center
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

			layoutGrid.Children.Add (title, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (busyIcon, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamSearch, 0, 1, spanYi++, spanYf++);
			layoutGrid.Children.Add (teamSearchEntry, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (teamSearchBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (matchSearch, 0, 1, spanYi++, spanYf++);
			layoutGrid.Children.Add (matchSearchEntry, 0, 1, spanYi, spanYf);
			layoutGrid.Children.Add (matchSearchBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (allMatchesBtn, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (allMatchesWarning, 1, 2, spanYi++, spanYf++);
			layoutGrid.Children.Add (navigationBtns, 0, 2, spanYi++, spanYf++);

			this.Content = new ScrollView () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async void filterMatches(int searchType, int number){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchData");
			ParseQuery<ParseObject> sorted = query.OrderBy("matchNo");
			ParseQuery<ParseObject> filter;
			IEnumerable<ParseObject> dataSelect = null;

			if (searchType == 1) {
				filter = sorted.WhereEqualTo ("teamNo", number);			
				dataSelect = await filter.FindAsync();
				try{
					await Navigation.PushModalAsync(new MatchDataDisplayPage(dataSelect, number));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			} else if (searchType == 2) {
				filter = query.WhereEqualTo ("matchNo", number);
				dataSelect = await filter.FindAsync();
				try{
					await Navigation.PushModalAsync(new MatchDataDisplayPage(dataSelect, number));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			} else if (searchType == 3){
				dataSelect = await sorted.FindAsync();
				try{
					await Navigation.PushModalAsync(new MatchDataDisplayPage(dataSelect, number));
				}
				catch{
					DisplayAlert("Error:", "Invalid search query", "OK");
				}
			}

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

