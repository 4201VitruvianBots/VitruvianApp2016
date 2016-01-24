using System;
using Xamarin.Forms;

namespace VitruvianApp2016
{
	public class ScrollViewTestPage : ContentPage
	{
		//StackLayout testStack = new StackLayout();
		ScrollView testHorizontalScroll = new ScrollView();
		ScrollView testVerticalScroll = new ScrollView();
		Grid testGrid = new Grid();
		Grid holderGrid = new Grid(){
			RowDefinitions = 
			{
				new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) }
			},
			ColumnDefinitions = 
			{
				new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) },
			}
		};
		Grid layoutGridTest = new Grid();

		public ScrollViewTestPage ()
		{
			layoutGridTest.Children.Add (testHorizontalScroll, 0, 0);
			testHorizontalScroll.Orientation = ScrollOrientation.Horizontal;
			testHorizontalScroll.Content = holderGrid;
			//holderGrid.Children.Add (testVerticalScroll, 0, 0);
			//testVerticalScroll.Content = testGrid;

			for(int i=0;i<100;i++){
				holderGrid.Children.Add(new Label(){Text = "Test Label: " + i},i,0);
				//testGrid.Children.Add(new Label(){Text = "Test Label: " + i},i,0);
				//Console.WriteLine ("Scroll Size: " + testScroll.ContentSize.ToString());
			}

			this.Content = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					layoutGridTest
				}
			};
		}
	}
}

