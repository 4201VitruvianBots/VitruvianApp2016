﻿using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class TeamListCell:ContentView
	{
		public Label teamName;

		public TeamListCell ()
		{
			teamName = new Label ();
			WidthRequest = 100;
			HeightRequest = 50;
			teamName.FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label));

			Content = teamName;
		}
	}
}

