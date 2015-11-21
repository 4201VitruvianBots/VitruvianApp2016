using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class ColumnCell:ContentView
	{
		public Label column; 

		public ColumnCell ()
		{
			column = new Label ();
			HeightRequest = 20;
			column.FontSize = GlobalVariables.sizeMedium;
			column.BackgroundColor = Color.Black;
			column.TextColor = Color.White;
			column.FontAttributes = FontAttributes.Bold;

			Content = column;
		}
	}
}

