using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchDataSelectPage:ContentPage
	{
		StackLayout dataList = new StackLayout();

		public MatchDataSelectPage ()
		{
			Grid layoutGrid = new Grid () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = GridLength.Auto }
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = GridLength.Auto },
					new ColumnDefinition{ Width = GridLength.Auto }
				}

			};

			Label title = new Label () {
				Text = "Match Data",
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
			layoutGrid.Children.Add (navigationBtns, 0, 2, 5, 6);

			this.Content = new ScrollView () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async Task filterMatches(int searchType, int number){
			ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchData");
			ParseQuery<ParseObject> sorted = query.OrderBy("matchNo");
			ParseQuery<ParseObject> filter;

			if (searchType == 1) {
				filter = sorted.WhereEqualTo ("teamNo", number);			
				var dataSelect = await filter.FindAsync();
				Console.WriteLine ("test1");
				await Navigation.PushModalAsync(new MatchDataViewTeamPage(dataSelect, number));
				Console.WriteLine ("test2");
			} else if (searchType == 2) {
				filter = query.WhereEqualTo ("matchNo", number);
				var dataSelect = await filter.FindAsync();
				//Navigation.PushAsync(new MatchDataViewMatchPage(dataSelect));
			}
		}
	}
}

