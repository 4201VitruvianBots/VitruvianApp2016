using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class StatComparsionPage:ContentPage
	{
		ActivityIndicator busyIcon = new ActivityIndicator ();

		ParseObject data;

		int gridX = 0;
		int gridY = 0;

		// autoD, autoS, teleOpD, teleOpS, other
		bool[] filterArray = new bool[3]{true, true, true};
		enum parameters {Low_Bar ,Portcullis, Cheval_de_Frise, Moat, Ramparts, Drawbridge, Salley_Port, Rock_Wall, Rough_Terrain, teleOpHighSuccesses, teleOpHighTotal, teleOpHighAccuracy, teleOpLowSuccesses, teleOpLowTotal, teleOpLowAccuracy, ShotsDenied, Teamwork, Scales, Fouls};
		enum parseString {E, A1, A2, B1, B2, C1, C2, D1, D2, teleOpHighSuccesses, teleOpHighTotal, teleOpHighAccuracy, teleOpLowSuccesses, teleOpLowTotal, teleOpLowAccuracy, totalShotsDenied, totalTeamworkCount, scales, totalFouls};

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

		public StatComparsionPage ()
		{

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
				HorizontalOptions = LayoutOptions.FillAndExpand,
				TextColor = Color.White,
				FontSize = GlobalVariables.sizeTitle,
				FontAttributes = FontAttributes.Bold,
				Text = "All Teams"
			};

			// Def Filter
			Button autoFilterBtn = new Button () {
				Text = "Show Def",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			autoFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[3]{true, false, false};
				populateData(null);
			};

			// Shot Filter
			Button teleOpFilterBtn = new Button () {
				Text = "Show Shots",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			teleOpFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[3]{false, true, false};
				populateData(null);
			};

			// Other Filter
			Button defenseFilterBtn = new Button () {
				Text = "Show Other",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			defenseFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[3]{false, false, true};
				populateData(null);
			};

			Picker parameterPicker = new Picker(){ 
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Title = "Select a Parameter"			
			};
			for (parameters i = parameters.Low_Bar; i <= parameters.Fouls; i++) {
				parameterPicker.Items.Add (i.ToString ());
			};
			parameterPicker.SelectedIndexChanged += (sender, e) => {
				parameters select = (parameters)parameterPicker.SelectedIndex;
				parameterPicker.Title = select.ToString();
			};

			// Sort Button
			Button sortBtn = new Button () {
				Text = "Sort by Parameter",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			sortBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus()){
					if(parameterPicker.SelectedIndex == -1)
						DisplayAlert("Error", "Select a Parameter", "OK");
					else
						populateData(((parseString)parameterPicker.SelectedIndex).ToString());
				}
			};


			//Reset Filters
			Button resetFilterBtn = new Button () {
				Text = "Reset Filters",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			resetFilterBtn.Clicked += (object sender, EventArgs e) => {
				filterArray = new bool[3]{true, true, true};
				populateData(null);
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
					resetFilterBtn,
					parameterPicker,
					sortBtn
				}
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				if (new CheckInternetConnectivity().InternetStatus())
					populateData(null);
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
			if (new CheckInternetConnectivity().InternetStatus())
				populateData (null);
		}

		async void populateData(string parseString){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;

			dataHolder.Children.Clear ();
			headerGrid.Children.Clear ();
			dataGrid.Children.Clear();
			dataGrid.RowDefinitions.Clear ();

			gridY = 0;
			gridX = 0;
			addColumnHeaders ("Team No.", gridX++);

			if (filterArray [0] == true) {
				addColumnHeaders ("Portcullis", gridX++);
				addColumnHeaders ("Cheval de Frise", gridX++);
				addColumnHeaders ("Moat", gridX++);
				addColumnHeaders ("Ramparts", gridX++);
				addColumnHeaders ("Drawbridge", gridX++);
				addColumnHeaders ("Sally Port", gridX++);
				addColumnHeaders ("Rock Wall", gridX++);
				addColumnHeaders ("Rough Terrain", gridX++);
				addColumnHeaders ("Low Bar", gridX++);
			}
			if (filterArray [1] == true) {
				addColumnHeaders ("T. Shot H. S.", gridX++);
				addColumnHeaders ("T. Shot H. T.", gridX++);
				addColumnHeaders ("T. Shot H. Acc.", gridX++);
				addColumnHeaders ("T. Shot L. S.", gridX++);
				addColumnHeaders ("T. Shot L. T.", gridX++);
				addColumnHeaders ("T. Shot L. Acc.", gridX++);
				addColumnHeaders ("Shots Denied", gridX++);
			}
			if (filterArray [2] == true) {
				addColumnHeaders ("Teamwork", gridX++);
				addColumnHeaders ("Scaled", gridX++);
				addColumnHeaders ("Fouls", gridX++);
			}
			ParseQuery<ParseObject> sorted;
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			if(parseString == null)
				sorted = query.OrderBy("teamNumber");
			else
				sorted = query.OrderByDescending(parseString);

			var allTeams = await sorted.FindAsync();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchIfNeededAsync ();
				data = obj;

				gridX = 0;

				dataGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)});

				addColumnData (data, "teamNumber", gridX++);

				if (filterArray [0] == true) {
					addColumnData (data, "A1", gridX++);
					addColumnData (data, "A2", gridX++);
					addColumnData (data, "B1", gridX++);
					addColumnData (data, "B2", gridX++);
					addColumnData (data, "C1", gridX++);
					addColumnData (data, "C2", gridX++);
					addColumnData (data, "D1", gridX++);
					addColumnData (data, "D2", gridX++);
					addColumnData (data, "E", gridX++);
				}
				if (filterArray [1] == true) {
					addColumnData (data, "teleOpHighSuccesses", gridX++);
					addColumnData (data, "teleOpHighTotal", gridX++);
					addColumnData (data, "teleOpHighAccuracy", gridX++);
					addColumnData (data, "teleOpLowSuccesses", gridX++);
					addColumnData (data, "teleOpLowTotal", gridX++);
					addColumnData (data, "teleOpLowAccuracy", gridX++);
					addColumnData (data, "totalShotsDenied", gridX++);
				}
				if (filterArray [2] == true) {
					addColumnData (data, "totalTeamworkCount", gridX++);
					addColumnData (data, "scales", gridX++);
					addColumnData (data, "totalFouls", gridX++);
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

