using System;
using Xamarin.Forms;
namespace VitruvianApp2016
{
	public class FunctionExample:ContentPage
	{
		int N = 99;		// N is just a large number
		int count = 0; // Counter for how many times you've implemented object(s) using a function
		Label[] labelArray = new Label[N];	//Creates an array of labels

		public FunctionExample ()
		{
			// Each function takes an argument, or parameter that is user-defined
			functionExample ("Label1");
			functionExample ("Label2");
			functionExample ("Label3");
			// function Example can be repeated as many times as needed for all your labels

			StackLayout pageLayout = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.Center
			};


			//Using a loop, add the labels to the page in the order you created them
			for (int i = 0; i < count; i++) {
				pageLayout.Children.Add (labelArray [i]);	
			}

			this.Content = pageLayout; //set the page to show the stack layout
		}

		void functionExample(string labelText){
			labelArray[Z] = new Label {
				Text = labelText,
				FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				TextColor = Color.Green
			};
			count++; // Increases the counter by 1 every time you use the funcition
		}
	}
}

