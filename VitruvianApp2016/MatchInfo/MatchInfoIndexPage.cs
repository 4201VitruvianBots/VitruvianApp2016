using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
//using TheBlueAlliance;

namespace VitruvianApp2016
{
	public class MatchInfoIndexPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		IEnumerable<ParseObject> matchList;
		ParseObject data;
		int teamNumber;

		Grid layoutGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = new GridLength(150, GridUnitType.Absolute)},
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid holderGrid = new Grid(){
			RowDefinitions = {
				new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
			}
		};
		Grid headerGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};
		Grid matchGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};

		ScrollView matchScroll = new ScrollView();

		int gridX = 0;
		int gridY = 0;

		public MatchInfoIndexPage (IEnumerable<ParseObject> matchListGet, int teamNo)
		{
			teamNumber = teamNo;
			matchList = matchListGet;	

			Label pageTitle = new Label () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Match List",
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				populateData();
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
					backBtn,
					refreshBtn
				}
			};

			layoutGrid.Children.Add (holderGrid, 0, 2, 1, 2);
			layoutGrid.Children.Add (pageTitle, 0, 0);
			layoutGrid.Children.Add (busyIcon, 1, 0);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					layoutGrid
				}
			};
			if (new CheckInternetConnectivity().InternetStatus())
				populateData ();
		}

		async void populateData(){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			//ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchList");
			//ParseQuery<ParseObject> sorted = matchList.OrderBy("matchNo");
			//var allMatches = await sorted.FindAsync();


			matchGrid.Children.Clear();
			matchGrid.RowDefinitions.Clear ();
			gridY = 0;
			gridX = 0;

			addColumnHeaders ("Match No.", gridX++);
			addColumnHeaders ("Red 1", gridX++);
			addColumnHeaders ("Red 2", gridX++);
			addColumnHeaders ("Red 3", gridX++);
			addColumnHeaders ("Blue 1", gridX++);
			addColumnHeaders ("Blue 2", gridX++);
			addColumnHeaders ("Blue 3", gridX++);

			foreach (ParseObject obj in matchList) {
				await obj.FetchIfNeededAsync();
				data = obj;

				matchGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute)});

				gridX = 0;

				addColumnData (data, "matchNo", gridX++);
				addColumnData (data, "red1", gridX++);
				addColumnData (data, "red2", gridX++);
				addColumnData (data, "red3", gridX++);
				addColumnData (data, "blue1", gridX++);
				addColumnData (data, "blue2", gridX++);
				addColumnData (data, "blue3", gridX++);

				gridY++;
			}
			matchScroll.Content = matchGrid;

			holderGrid.Children.Add (headerGrid, 0, 0);
			holderGrid.Children.Add (matchScroll, 0, 1);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		void addColumnHeaders(string headerName, int arrayIndex){
			ColumnHeaderCell dataHeader = new ColumnHeaderCell ();
			dataHeader.header.Text = headerName;

			headerGrid.Children.Add (dataHeader, arrayIndex, 0);
		}

		void addColumnData(ParseObject data, string parseString, int arrayIndex){
			DataCell cell = new DataCell ();
			cell.data.TextColor = Color.White;
			try{
				cell.data.Text = data[parseString].ToString();
				if(data[parseString].ToString() == teamNumber.ToString()){
					cell.data.TextColor = Color.Lime;
					cell.data.FontSize = GlobalVariables.sizeTitle;
					cell.data.FontAttributes = FontAttributes.Bold;
				}
			}
			catch{
				cell.data.Text = "NULL";
			}
			if (gridX == 1)
				cell.BackgroundColor = Color.Olive;
			if (gridY % 2 == 1 && gridX < 5 && gridX != 1)
				cell.BackgroundColor = Color.Maroon;
			else if (gridY % 2 == 1 && gridX > 4)
				cell.BackgroundColor = Color.Teal;
			if (gridY % 2 == 0 && gridX < 5 && gridX != 1)
				cell.BackgroundColor = Color.Red;
			else if (gridY % 2 == 0 && gridX > 4)
				cell.BackgroundColor = Color.Blue;

			if (arrayIndex == 0) {
				TapGestureRecognizer tap = new TapGestureRecognizer ();
				tap.Tapped += (object sender, EventArgs e) => {
					if (new CheckInternetConnectivity().InternetStatus())
						Navigation.PushModalAsync (new MatchInfoDisplayPage (data));
				};
				cell.GestureRecognizers.Add (tap);
			}

			matchGrid.Children.Add (cell, arrayIndex, gridY);


			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

