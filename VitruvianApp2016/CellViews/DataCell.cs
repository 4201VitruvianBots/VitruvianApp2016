using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class DataCell:ContentView
	{
		public Label data;
		
		public DataCell ()
		{
			data = new Label ();
			WidthRequest = 30;
			HeightRequest = 10;
			BackgroundColor = Color.White;
			data.TextColor = Color.Black;
			data.FontSize = GlobalVariables.sizeSmall;
			data.XAlign = TextAlignment.End;

			Content = data;
		}
	}
}

