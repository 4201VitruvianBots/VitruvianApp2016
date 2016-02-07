using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchDataDisplayPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		IEnumerable<ParseObject> dataSelect;
		ParseObject data;

		int gridX = 0;
		int gridY = 0;

		// autoD, autoS, teleOpD, teleOpS, other
		bool[] filterArray = new bool[5]{true, true, true, true, true};


		Grid headerGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};

		Grid dataGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor = Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,
		};


		ScrollView dataVerticalScroll = new ScrollView ();
		ScrollView dataHorizontalScroll;

		Grid dataHolder = new Grid(){
			RowDefinitions = 
			{
				new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
			}
		};

		public MatchDataDisplayPage (IEnumerable<ParseObject> data, int number)
		{
			dataSelect = data;

			Grid layoutGrid = new Grid () {
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
					
			Label titleLabel = new Label {
				BackgroundColor = Color.Black,
				TextColor = Color.Green,
				FontSize = GlobalVariables.sizeTitle
			};
			if (number == -4201)
				titleLabel.Text = "All Matches";
			else
				titleLabel.Text = "Team " + number;

			// Auto Filter
			Button autoFilterBtn = new Button () {
				Text = "Show Auto",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			autoFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{true, true, false, false, false};
				populateData();
			};

			// TeleOp Filter
			Button teleOpFilterBtn = new Button () {
				Text = "Show TeleOp",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			teleOpFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{false, false, true, true, false};
				populateData();
			};

			// Defense Filter
			Button defenseFilterBtn = new Button () {
				Text = "Show Defense",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			defenseFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{true, false, true, false, false};
				populateData();
			};

			// Shoot Filter
			Button shotFilterBtn = new Button () {
				Text = "Show Shots",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			shotFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{false, true, false, true, false};
				populateData();
			};

			// Misc Filter
			Button miscFilterBtn = new Button () {
				Text = "Show Other",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			miscFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{false, false, false, false, true};
				populateData();
			};

			//Reset Filters
			Button resetFilterBtn = new Button () {
				Text = "Reset Filters",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			resetFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[5]{true, true, true, true, true};
				populateData();
			};

			// Filter Buttons
			StackLayout filterBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					autoFilterBtn,
					teleOpFilterBtn,
					defenseFilterBtn,
					shotFilterBtn,
					miscFilterBtn,
					resetFilterBtn
				}
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

			dataHorizontalScroll = new ScrollView () {
				Orientation = ScrollOrientation.Horizontal,

				Content = dataHolder
			};

			Console.WriteLine ("Scroll Size: " + dataVerticalScroll.ContentSize.ToString());

			layoutGrid.Children.Add (dataHorizontalScroll, 0, 2, 1, 2);
			layoutGrid.Children.Add (titleLabel, 0, 0);
			layoutGrid.Children.Add (busyIcon, 1, 0);
			layoutGrid.Children.Add (filterBtns, 0, 2, 2, 3);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 3, 4);

			this.Content = new StackLayout {
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

			dataHolder.Children.Clear ();
			headerGrid.Children.Clear ();
			dataGrid.Children.Clear();
			dataGrid.RowDefinitions.Clear ();

			gridY = 0;
			gridX = 0;
			addColumnHeaders ("Match No.", gridX++);
			addColumnHeaders ("Team No.", gridX++);

			if (filterArray [0] == true) {
				addColumnHeaders ("Auto A1", gridX++);
				addColumnHeaders ("Auto A2", gridX++);
				addColumnHeaders ("Auto B1", gridX++);
				addColumnHeaders ("Auto B2", gridX++);
				addColumnHeaders ("Auto C1", gridX++);
				addColumnHeaders ("Auto C2", gridX++);
				addColumnHeaders ("Auto D1", gridX++);
				addColumnHeaders ("Auto D2", gridX++);
				addColumnHeaders ("Auto E", gridX++);
			}
			if (filterArray [1] == true) {
				addColumnHeaders ("A. Shot H. S.", gridX++);
				addColumnHeaders ("A. Shot H. T.", gridX++);
				addColumnHeaders ("A. Shot H. Acc.", gridX++);
				addColumnHeaders ("A. Shot L. S.", gridX++);
				addColumnHeaders ("A. Shot L. T.", gridX++);
			}
			if (filterArray [2] == true) {
				addColumnHeaders ("TeleOp A1", gridX++);
				addColumnHeaders ("TeleOp A2", gridX++);
				addColumnHeaders ("TeleOp B1", gridX++);
				addColumnHeaders ("TeleOp B2", gridX++);
				addColumnHeaders ("TeleOp C1", gridX++);
				addColumnHeaders ("TeleOp C2", gridX++);
				addColumnHeaders ("TeleOp D1", gridX++);
				addColumnHeaders ("TeleOp D2", gridX++);
				addColumnHeaders ("TeleOp E", gridX++);
			}
			if (filterArray [3] == true) {
				addColumnHeaders ("T. Shot H. S.", gridX++);
				addColumnHeaders ("T. Shot H. T.", gridX++);
				addColumnHeaders ("T. Shot H. Acc.", gridX++);
				addColumnHeaders ("T. Shot L. S.", gridX++);
				addColumnHeaders ("T. Shot L. T.", gridX++);
				addColumnHeaders ("Shots Denied", gridX++);
			}
			if (filterArray [4] == true) {
				addColumnHeaders ("Challenged", gridX++);
				addColumnHeaders ("Scaled", gridX++);
				addColumnHeaders ("Disabled", gridX++);
			}
				
			foreach (ParseObject obj in dataSelect) {
				await obj.FetchIfNeededAsync ();
				data = obj;

				gridX = 0;

				dataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)});

				addColumnData (data, "matchNo", gridX++);
				addColumnData (data, "teamNo", gridX++);

				if (filterArray [0] == true) {
					addColumnData (data, "autoA1", gridX++);
					addColumnData (data, "autoA2", gridX++);
					addColumnData (data, "autoB1", gridX++);
					addColumnData (data, "autoB2", gridX++);
					addColumnData (data, "autoC1", gridX++);
					addColumnData (data, "autoC2", gridX++);
					addColumnData (data, "autoD1", gridX++);
					addColumnData (data, "autoD2", gridX++);
					addColumnData (data, "autoE", gridX++);
				}
				if (filterArray [1] == true) {
					addColumnData (data, "autoShotHighSuccess", gridX++);
					addColumnData (data, "autoShotHighTotal", gridX++);
					addColumnData (data, "autoShotHighAccuracy", gridX++);
					addColumnData (data, "autoShotLowSuccess", gridX++);
					addColumnData (data, "autoShotLowTotal", gridX++);
				}
				if (filterArray [2] == true) {
					addColumnData (data, "teleOpA1", gridX++);
					addColumnData (data, "teleOpA2", gridX++);
					addColumnData (data, "teleOpB1", gridX++);
					addColumnData (data, "teleOpB2", gridX++);
					addColumnData (data, "teleOpC1", gridX++);
					addColumnData (data, "teleOpC2", gridX++);
					addColumnData (data, "teleOpD1", gridX++);
					addColumnData (data, "teleOpD2", gridX++);
					addColumnData (data, "teleOpE", gridX++);
				}
				if (filterArray [3] == true) {
					addColumnData (data, "teleOpShotHighSuccess", gridX++);
					addColumnData (data, "teleOpShotHighTotal", gridX++);
					addColumnData (data, "teleOpShotHighAccuracy", gridX++);
					addColumnData (data, "teleOpShotLowSuccess", gridX++);
					addColumnData (data, "teleOpShotLowTotal", gridX++);
					addColumnData (data, "shotsDenied", gridX++);
				}
				if (filterArray [4] == true) {
					addColumnData (data, "challenged", gridX++);
					addColumnData (data, "scaled", gridX++);
					addColumnData (data, "disabled", gridX++);
				}

				gridY++;
			}
			dataVerticalScroll.Content = dataGrid;

			dataHolder.Children.Add (headerGrid, 0, 0);
			dataHolder.Children.Add (dataVerticalScroll, 0, 1);

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
			try{
				cell.data.Text = data[parseString].ToString();
			}
			catch{
				cell.data.Text = "NULL";
			}

			if (gridY % 2 == 1)
				cell.BackgroundColor = Color.Gray;

			dataGrid.Children.Add (cell, arrayIndex, gridY);
		}
	}
}

