using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class ColumnHeader:ContentView
	{
		public Label header; 

		public ColumnHeader ()
		{
			header = new Label ();
			HeightRequest = 10;
			header.FontSize = GlobalVariables.sizeMedium;
			header.BackgroundColor = Color.Black;
			header.TextColor = Color.White;
			header.FontAttributes = FontAttributes.Bold;

			Content = header;
		}
	}
}

