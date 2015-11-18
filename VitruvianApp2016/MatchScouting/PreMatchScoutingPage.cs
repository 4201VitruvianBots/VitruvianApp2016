using System;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class PreMatchScoutingPage:ContentPage
	{
		ParseObject MatchData = new ParseObject("MatchData");

		public PreMatchScoutingPage ()
		{
			Label matchNoLabel = new Label {
				Text = "Match Number:",
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry matchNo = new Entry {
				Placeholder = "[Enter Match No.]"
			};
			matchNo.Keyboard = Keyboard.Numeric;

			Label teamNoLabel = new Label {
				Text = "Team Number:",
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
			};

			Entry teamNo = new Entry {
				Placeholder = "[Enter Team No.]"
			};
			teamNo.Keyboard = Keyboard.Numeric;
			//Start Match Scout

			Button beginScoutBtn = new Button {
				Text = "Begin Match",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			beginScoutBtn.Clicked += (object sender, EventArgs e) => {
				if(string.IsNullOrEmpty(matchNo.Text) || string.IsNullOrEmpty(teamNo.Text)){
					DisplayAlert("Error", "Input both Team Number and Match Number", "Ok");
				} else {
					Console.WriteLine(matchNo.Text);
					MatchData.Add("team_Match", teamNo.Text+"-"+matchNo.Text);
					MatchData.Add("teamNo", Convert.ToInt32(teamNo.Text));
					MatchData.Add("matchNo", Convert.ToInt32(matchNo.Text));
					SaveData();
					Navigation.PushModalAsync(new AutoMatchScoutingPage(MatchData));
				}
			};


			//Back Button
			Button backBtn = new Button (){
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				Navigation.PushModalAsync(new MainMenuPage()); // This must be push because stack of pages will be different after a full match scout
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,

				Children = {
					backBtn,
					beginScoutBtn
				}
			};

			this.Content = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					matchNoLabel,
					matchNo,
					teamNoLabel,
					teamNo,
					navigationBtns
				}
			};
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await MatchData.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}
