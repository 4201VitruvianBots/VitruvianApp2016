using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class CoachViewDisplayPage: ContentPage
	{
		ParseObject data;

		public CoachViewDisplayPage(ParseObject Match)
		{
			data = Match;

			StackLayout[] TeamData = new StackLayout[6] ();
				DataLayout (data ["red1"], TeamData [0]);
			DataLayout (data ["red2"], TeamData [1]);
			DataLayout (data ["red3"], TeamData [2]);
			DataLayout (data ["blue1"], TeamData [3]);
			DataLayout (data ["blue2"], TeamData [4]);
			DataLayout (data ["blue3"], TeamData [5]);

			StackLayout VerticalLayout  = new StackLayout () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Vertical
			};
			for (int i = 0; i < 6; i++) {
				VerticalLayout.Children.Add (TeamData [i]);
			};
			this.Content = VerticalLayout;


		}
		void DataLayout(ParseObject team, StackLayout stack){
			Label teamnumber = new Label () {
				Text = team ["TeamNo."].ToString ()
			};
			Label teamname = new Label () {
				Text = team ["TeamName"].ToString ()
			};
			stack.Children.Add (teamnumber);
			stack.Children.Add (teamname);
			//repeat for all other needed data

					

			
	}
}

