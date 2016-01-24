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

		int X = 0;
		int Y = 0;

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

			for (int i = 0; i < 30; i++)
				dataGrid.RowDefinitions = new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute) };
			
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
			Y = 1;
			X = 0;
			addColumnHeaders ("Match No.", X++);
			addColumnHeaders ("Team No.", X++);

			if (filterArray [0] == true) {
				addColumnHeaders ("Auto A1", X++);
				addColumnHeaders ("Auto A2", X++);
				addColumnHeaders ("Auto B1", X++);
				addColumnHeaders ("Auto B2", X++);
				addColumnHeaders ("Auto C1", X++);
				addColumnHeaders ("Auto C2", X++);
				addColumnHeaders ("Auto D1", X++);
				addColumnHeaders ("Auto D2", X++);
				addColumnHeaders ("Auto E", X++);
			}
			if (filterArray [1] == true) {
				addColumnHeaders ("A. Shot H. S.", X++);
				addColumnHeaders ("A. Shot H. T.", X++);
				addColumnHeaders ("A. Shot L. S.", X++);
				addColumnHeaders ("A. Shot L. T.", X++);
			}
			if (filterArray [2] == true) {
				addColumnHeaders ("TeleOp A1", X++);
				addColumnHeaders ("TeleOp A2", X++);
				addColumnHeaders ("TeleOp B1", X++);
				addColumnHeaders ("TeleOp B2", X++);
				addColumnHeaders ("TeleOp C1", X++);
				addColumnHeaders ("TeleOp C2", X++);
				addColumnHeaders ("TeleOp D1", X++);
				addColumnHeaders ("TeleOp D2", X++);
				addColumnHeaders ("TeleOp E", X++);
			}
			if (filterArray [3] == true) {
				addColumnHeaders ("T. Shot H. S.", X++);
				addColumnHeaders ("T. Shot H. T.", X++);
				addColumnHeaders ("T. Shot L. S.", X++);
				addColumnHeaders ("T. Shot L. T.", X++);
				addColumnHeaders ("Shots Denied", X++);
			}
			if (filterArray [4] == true) {
				addColumnHeaders ("Challenged", X++);
				addColumnHeaders ("Scaled", X++);
				addColumnHeaders ("Disabled", X++);
			}
				
			foreach (ParseObject obj in dataSelect) {
				await obj.FetchIfNeededAsync ();
				data = obj;

				X = 0;

				addColumnData (data, "matchNo", X++);
				addColumnData (data, "teamNo", X++);

				if (filterArray [0] == true) {
					addColumnData (data, "autoA1", X++);
					addColumnData (data, "autoA2", X++);
					addColumnData (data, "autoB1", X++);
					addColumnData (data, "autoB2", X++);
					addColumnData (data, "autoC1", X++);
					addColumnData (data, "autoC2", X++);
					addColumnData (data, "autoD1", X++);
					addColumnData (data, "autoD2", X++);
					addColumnData (data, "autoE", X++);
				}
				if (filterArray [1] == true) {
					addColumnData (data, "autoShotHighSuccess", X++);
					addColumnData (data, "autoShotHighTotal", X++);
					addColumnData (data, "autoShotLowSuccess", X++);
					addColumnData (data, "autoShotLowTotal", X++);
				}
				if (filterArray [2] == true) {
					addColumnData (data, "teleOpA1", X++);
					addColumnData (data, "teleOpA2", X++);
					addColumnData (data, "teleOpB1", X++);
					addColumnData (data, "teleOpB2", X++);
					addColumnData (data, "teleOpC1", X++);
					addColumnData (data, "teleOpC2", X++);
					addColumnData (data, "teleOpD1", X++);
					addColumnData (data, "teleOpD2", X++);
					addColumnData (data, "teleOpE", X++);
				}
				if (filterArray [3] == true) {
					addColumnData (data, "teleOpShotHighSuccess", X++);
					addColumnData (data, "teleOpShotHighTotal", X++);
					addColumnData (data, "teleOpShotLowSuccess", X++);
					addColumnData (data, "teleOpShotLowTotal", X++);
					addColumnData (data, "shotsDenied", X++);
				}
				if (filterArray [4] == true) {
					addColumnData (data, "challenged", X++);
					addColumnData (data, "scaled", X++);
					addColumnData (data, "disabled", X++);
				}

				Y++;
			}
			dataVerticalScroll.Content = dataGrid;

			dataHolder.Children.Add (headerGrid, 0, 0);
			dataHolder.Children.Add (dataVerticalScroll, 0, 1);

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

			if (Y % 2 == 0)
				cell.BackgroundColor = Color.Gray;

			dataGrid.Children.Add (cell, arrayIndex, Y);
		}
	}
}

