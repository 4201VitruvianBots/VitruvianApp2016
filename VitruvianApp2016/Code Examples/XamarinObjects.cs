using System;
using Xamarin.Forms;
namespace VitruvianApp2016
{
	public class XamarinObjects:ContentPage
	{
		// Enumerated values for picker example
		enum Day{Sun, Mon, Tue, Wed, Thur, Fri, Sat};

		public XamarinObjects ()
		{
			// Labels
			Label labelExample1 = new Label () {
				TextColor = Color.Green,
				HorizontalOptions = LayoutOptions.Center,
				Text = "Sample Text",
				FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label))
			};

			Label labelExample2 = new Label ();
			labelExample2.TextColor = Color.Green;
			labelExample2.HorizontalOptions = LayoutOptions.Center;
			labelExample2.Text = "Sample Text";
			labelExample2.FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label));

			//Buttons
			Button buttonExample = new Button () {
				Text = "Button Text",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			buttonExample.Clicked += (object sender, EventArgs e) => {
				//Navigation.PushModalAsync (new "page"); // Opens a new page on the App
				//Navigation.PopModalAsync()	// Return to the previous page
			};

			// Entry
			Entry entryExample = new Entry () {
				Text = "Sample Text",
				Keyboard = Keyboard.Numeric
			};

			// Editor
			Editor editorExample = new Editor () {
				Text = "Sample Text",
				HeightRequest = 200
			};
			// Picker
			var dateSelect = new Picker();
			dateSelect.Title = "Select a Day";
			for (Day i = Day.Sun; i < day.Sat; i++) {
				dateSelect.Items.Add (i.ToString ());	//Populate the picker with all of the day choices
			}
			dateSelect.SelectedIndexChanged	 +=(sender, e) => {
				dateSelect.Title = dateSelect.SelectedIndex.ToString();	// if the user selects a day, change the title to show the selected day
			};

			//DisplayAlert
			bool test;
			Button alertExample = new Button () {
				Text = "Button Text",
				TextColor = Color.Green,
				BackgroundColor = Color.Black,
				FontSize = Device.GetNamedSize (NamedSize.Medium, typeof(Label))
			};
			alertExample.Clicked += (object sender, EventArgs e) => {
				if(test== true){													//If a paramenter you set is equal to true, display the message/error to the user
					DisplayAlert("Message Title", "Message", "OK");
				}
			};

		}
	}
}

