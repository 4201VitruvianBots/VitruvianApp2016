using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class TeamNumberCell:ContentView
	{
		public Label teamNo;

		public TeamNumberCell ()
		{
			teamNo = new Label ();
			WidthRequest = 40;
			HeightRequest = 20;
			BackgroundColor = Color.White;
			teamNo.TextColor = Color.Black;
			teamNo.FontSize = GlobalVariables.sizeSmall;
			teamNo.XAlign = TextAlignment.End;

			Content = teamNo;
		}
	}
}

