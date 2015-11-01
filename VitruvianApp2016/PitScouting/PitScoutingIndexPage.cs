using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;

namespace VitruvianApp2016
{
	public class PitScoutingIndexPage:ContentPage
	{
		StackLayout teamStack = new StackLayout();

		public PitScoutingIndexPage ()
		{
		}

		async Task UpdateTeamList(){
			ParseQuery<ParseObject> query = ParseObject.GetQuery("TeamData");
			ParseQuery<ParseObject> sorted = query.OrderBy("teamNumber");

			var allTeams = await sorted.FindAsync();
			teamStack.Children.Clear();
			foreach (ParseObject obj in allTeams) {
				await obj.FetchAsync ();
				TeamListCell cell = new TeamListCell ();
				cell.teamName.Text = "Team " + obj ["teamNumber"];
				teamStack.Children.Add (cell);
				TapGestureRecognizer tap = new TapGestureRecognizer ();
				tap.Tapped += (object sender, EventArgs e) => {
					Navigation.PushModalAsync (new PitScoutingEditPage (obj));
				};
				cell.GestureRecognizers.Add (tap);
			}
		}
	}
}

