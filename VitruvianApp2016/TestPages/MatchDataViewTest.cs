using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class MatchDataViewTest : ContentPage
	{
		StackLayout teamStack = new StackLayout();

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

		public MatchDataViewTest ()
		{
			//Page Title
			Label title = new Label () {
				Text = "MatchDataViewTest",
				FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};

			//Refresh Button
			Button refreshBtn = new Button () {
				Text = "Refresh",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			refreshBtn.Clicked += (object sender, EventArgs e) => {
				UpdateTeamList();
			};

			//Back Button
			Button backBtn = new Button (){
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = GlobalVariables.sizeMedium
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync();
			};

			ScrollView teamList = new ScrollView ();
			teamList.Content = teamStack;
			this.Appearing += (object sender, EventArgs e) => {
				UpdateTeamList();
			};

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

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					title,
					teamStack,
					navigationBtns
				}
			};
		}
		async Task UpdateTeamList(){
			ParseQuery<ParseObject> query = ParseObject.GetQuery("MatchData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNo");
			ParseQuery<ParseObject> filter = sorted.WhereEqualTo ("teamNo", 41);

			var allTeams = await filter.FindAsync();
			teamStack.Children.Clear();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchAsync ();
				TeamListCell cell = new TeamListCell ();
				cell.teamName.Text = "Match " + obj ["matchNo"];
				teamStack.Children.Add (cell);
			}
		}
	}
}

