using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VitruvianApp2016
{
	public class MatchInfoIndexPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		IEnumerable<ParseObject> matchList;
		ParseObject data;

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

		int X = 0;
		int Y = 0;

		public MatchInfoIndexPage (IEnumerable<ParseObject> matchListGet)
		{
			matchList = matchListGet;	

			Label pageTitle = new Label () {
				Text = "Match List",
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeTitle
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				populateData();
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
			Y = 0;
			X = 0;
			addColumnHeaders ("Match No.", X++);
			addColumnHeaders ("Red 1", X++);
			addColumnHeaders ("Red 2", X++);
			addColumnHeaders ("Red 3", X++);
			addColumnHeaders ("Blue 1", X++);
			addColumnHeaders ("Blue 2", X++);
			addColumnHeaders ("Blue 3", X++);

			foreach (ParseObject obj in matchList) {
				await obj.FetchIfNeededAsync ();
				data = obj;

				matchGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength (50, GridUnitType.Absolute)});

				X = 0;

				addColumnData (data, "matchNo", X++);
				addColumnData (data, "red1", X++);
				addColumnData (data, "red2", X++);
				addColumnData (data, "red3", X++);
				addColumnData (data, "blue1", X++);
				addColumnData (data, "blue2", X++);
				addColumnData (data, "blue3", X++);

				Y++;
			}
			matchScroll.Content = matchGrid;

			holderGrid.Children.Add (headerGrid, 0, 0);
			holderGrid.Children.Add (matchScroll, 0, 1);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}

		void addColumnHeaders(string headerName, int arrayIndex){
			ColumnHeader dataHeader = new ColumnHeader ();
			dataHeader.header.Text = headerName;

			headerGrid.Children.Add (dataHeader, arrayIndex, 0);
		}

		void addColumnData(ParseObject data, string parseString, int arrayIndex){
			DataCell cell = new DataCell ();
			try{
				cell.data.Text = data[parseString].ToString();
			}
			catch{
				cell.data.Text = "NULL";
			}

			if (Y % 2 == 1)
				cell.BackgroundColor = Color.Gray;
			if (X == 1)
				cell.BackgroundColor = Color.Green;

			if (arrayIndex == 0) {
				TapGestureRecognizer tap = new TapGestureRecognizer ();
				tap.Tapped += (object sender, EventArgs e) => {
					Navigation.PushModalAsync (new MatchInfoDisplayPage (data));
				};
				cell.GestureRecognizers.Add (tap);
			}

			matchGrid.Children.Add (cell, arrayIndex, Y);


			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;
		}
	}
}

