using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class MatchNumberCell:ContentView
	{
		public Label matchNo;

		public MatchNumberCell ()
		{
			matchNo = new Label ();
			WidthRequest = 30;
			HeightRequest = 20;
			BackgroundColor = Color.White;
			matchNo.TextColor = Color.Black;
			matchNo.FontSize = GlobalVariables.sizeSmall;
			matchNo.XAlign = TextAlignment.End;

			Content = matchNo;
		}
	}
}

