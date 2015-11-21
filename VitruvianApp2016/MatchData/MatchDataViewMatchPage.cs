using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class MatchDataViewMatchPage:ContentPage
	{
		StackLayout dataList = new StackLayout();

		IEnumerable<ParseObject> dataSelect;
		int Z = 0;
		Grid dataGrid = new Grid(){
			HorizontalOptions = LayoutOptions.FillAndExpand,
			VerticalOptions = LayoutOptions.FillAndExpand,

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
				new RowDefinition{ Height = GridLength.Auto }
			},
			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto},
				new ColumnDefinition{ Width = GridLength.Auto}
			}

		};

		public MatchDataViewMatchPage (IEnumerable<ParseObject> data)
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

			ScrollView verticalScroll = new ScrollView () {
				Orientation = ScrollOrientation.Vertical,

				Content = dataGrid
			};

			//layoutGrid.Children.Add (top, 0, 2, 2, 3);
			layoutGrid.Children.Add (verticalScroll, 0, 2, 1, 2);
			layoutGrid.Children.Add (navigationBtns, 0, 2, 2, 3);

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = layoutGrid
			};
		}

		async void populateData(){
			dataList.Children.Clear();
			foreach (ParseObject obj in dataSelect) {
				await obj.FetchAsync ();
				MatchNumberCell cell1 = new MatchNumberCell ();
				cell1.matchNo.Text = obj ["matchNo"].ToString();
				dataGrid.Children.Add (cell1, 0, 1, Z, Z + 1);

				TeamNumberCell cell2 = new TeamNumberCell ();
				cell2.teamNo.Text = obj ["teamNo"].ToString();
				dataGrid.Children.Add (cell1, 0, 1, Z, Z + 1);

				Z++;
			}

		}
	}
}

