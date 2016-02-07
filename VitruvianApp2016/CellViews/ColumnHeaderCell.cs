using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class ColumnHeaderCell:ContentView
	{
		public Label header; 

		public ColumnHeaderCell ()
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

