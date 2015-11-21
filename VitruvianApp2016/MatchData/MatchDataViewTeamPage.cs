using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchDataViewTeamPage:ContentPage
	{
		StackLayout dataList = new StackLayout();

		ActivityIndicator busyIcon = new ActivityIndicator ();

		IEnumerable<ParseObject> dataSelect;

		int Z=1;

		Grid dataGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,
			BackgroundColor =Color.Black,
			ColumnSpacing = 1,
			RowSpacing = 1,

			RowDefinitions = {
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
				new RowDefinition{ Height = GridLength.Auto },
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto}
			}

		};


		public MatchDataViewTeamPage (IEnumerable<ParseObject> data, int teamNo)
		{
			dataSelect = data;
			populateData ();

			Grid layoutGrid = new Grid () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				RowDefinitions = {
					new RowDefinition{ Height = GridLength.Auto },
					new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },
					new RowDefinition{ Height = GridLength.Auto },
				},
				ColumnDefinitions = {
					new ColumnDefinition{ Width = GridLength.Auto},
					new ColumnDefinition{ Width = GridLength.Auto},
				}
			};

			Label teamNumberLabel = new Label {
				Text = "Team " + teamNo,
				BackgroundColor = Color.Black,
				TextColor = Color.Green,
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

			StackLayout dataStack = new StackLayout () {
				Children = {
					busyIcon,
					dataGrid
				}
			};

			ScrollView verticalScroll = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,

				Content = dataStack
			};

			//grid.Children.Add (robotImage, 0, 0);
			layoutGrid.Children.Add (teamNumberLabel, 1, 0);
			layoutGrid.Children.Add (verticalScroll, 0, 2, 1, 2);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async Task populateData(){
			busyIcon.IsVisible = true;
			busyIcon.IsRunning = true;
			dataGrid.Children.Clear();
			Z = 1;
			addColumnHeaders ();
			foreach (ParseObject obj in dataSelect) {
				await obj.FetchAsync ();

				MatchNumberCell cell1 = new MatchNumberCell ();
				cell1.matchNo.Text = obj ["matchNo"].ToString();
				dataGrid.Children.Add (cell1, 0, 1, Z, Z+1);

				TeamNumberCell cell2 = new TeamNumberCell ();
				cell2.teamNo.Text = obj ["teamNo"].ToString();
				dataGrid.Children.Add (cell2, 1, 2, Z, Z+1);

				DataCell cell3 = new DataCell ();
				cell3.data.Text = obj ["TotalScore"].ToString();
				dataGrid.Children.Add (cell3, 2, 3, Z, Z+1);

				DataCell cell4 = new DataCell ();
				cell4.data.Text = obj ["autoPoints"].ToString();
				dataGrid.Children.Add (cell4, 3, 4, Z, Z+1);

				DataCell cell5 = new DataCell ();
				cell5.data.Text = Convert.ToString(Convert.ToInt32(obj ["TotalScore"].ToString()) - Convert.ToInt32(obj ["autoPoints"].ToString()));
				dataGrid.Children.Add (cell5, 4, 5, Z, Z + 1);

				Z++;
			}

			dataList.Children.Add (dataGrid);

			busyIcon.IsVisible = false;
			busyIcon.IsRunning = false;

		}

		void addColumnHeaders(){
			ColumnCell matchColumn = new ColumnCell ();
			matchColumn.column.Text = "Match No.";
			ColumnCell teamColumn = new ColumnCell ();
			teamColumn.column.Text = "Team No.";
			ColumnCell scoreColumn = new ColumnCell ();
			scoreColumn.column.Text = "Total Score";
			ColumnCell autoColumn = new ColumnCell ();
			autoColumn.column.Text = "Auto Points";
			ColumnCell teleOpColumn = new ColumnCell ();
			teleOpColumn.column.Text = "TeleOp Points";

			dataGrid.Children.Add (matchColumn, 0, 0);
			dataGrid.Children.Add (teamColumn, 1, 0);
			dataGrid.Children.Add (scoreColumn, 2, 0);
			dataGrid.Children.Add (autoColumn, 3, 0);
			dataGrid.Children.Add (teleOpColumn, 4, 0);
		}
	}
}

