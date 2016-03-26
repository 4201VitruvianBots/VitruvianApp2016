using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class ColumnHeaderCellSmall:ContentView
	{
		public Label header; 

		public ColumnHeaderCellSmall ()
		{
			header = new Label ();
			HeightRequest = 10;
			header.FontSize = GlobalVariables.sizeSmall;
			header.BackgroundColor = Color.Black;
			header.TextColor = Color.White;
			header.FontAttributes = FontAttributes.Bold;
			header.HorizontalOptions = LayoutOptions.FillAndExpand;

			Content = header;
		}
	}
}

