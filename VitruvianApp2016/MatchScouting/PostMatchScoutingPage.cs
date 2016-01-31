using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class PostMatchScoutingPage:ContentPage
	{
		ParseObject data;

		enum RobotRole{Shooter, Breecher, Feeder, Other};
		enum Choice{No, Yes};

		string errorString = "The following data was unable to be saved: ";
		bool errorStatus = false;

		public PostMatchScoutingPage (ParseObject matchData)
		{
			Label interferenceLabel = new Label {
				Text = "Did the team interfere with their alliance members?",
				FontSize = GlobalVariables.sizeMedium,
			};

			Picker interferencePicker = new Picker();
			for(Choice type=Choice.No; type<=Choice.Yes; type++){
				string stringValue = type.ToString();
				interferencePicker.Items.Add(stringValue);
			}

			interferencePicker.SelectedIndexChanged += (sender, args) => {
				Choice type = (Choice)interferencePicker.SelectedIndex;
				string stringValue = type.ToString();
				interferencePicker.Title = stringValue;
			};

			Label roleLabel = new Label () {
				Text = "What role did this robot generally perform?:",
				FontSize = GlobalVariables.sizeMedium
			};
			Picker rolePicker = new Picker();
			rolePicker.Title = "Choose an Option";
			for (RobotRole i = RobotRole.Shooter; i <= RobotRole.Other; i++) {
				rolePicker.Items.Add (i.ToString ());
			};
			rolePicker.SelectedIndexChanged += (sender, e) => {
				rolePicker.Title = rolePicker.SelectedIndex.ToString ();
			};

			Label techFoulLabel = new Label {
				Text = "How many tech fouls did this team commit?:",
				FontSize = GlobalVariables.sizeMedium
			};

			Entry techFoulEntry = new Entry {
				Placeholder = "0",
				Keyboard = Keyboard.Numeric
			};

			Label noteLabel = new Label {
				Text = "Match comments/notes:",
				FontSize = GlobalVariables.sizeMedium
			};

			Editor noteEditor = new Editor{
				HeightRequest = 100,
				Text = "[notes]"
			};

			data = matchData;

			Button submit = new Button {
				Text = "Submit",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			submit.Clicked += (object sender, EventArgs e) => {
				if(interferencePicker.Title == "Yes")
					errorHandling("interference", true);
				else
					errorHandling("interference", false);
				if(noteEditor.Text != "[notes]" || string.IsNullOrEmpty(noteEditor.Text) || string.IsNullOrWhiteSpace(noteEditor.Text))
					errorHandling("matchNotes", noteEditor.Text);
				if(rolePicker.Title.ToString() != "Choose an Option")
					errorHandling("robotRole", rolePicker.Title.ToString());

				if(errorStatus == true){
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
				} else {
					Navigation.PushModalAsync(new PreMatchScoutingPage());
				}
			};

			Label keyboardPadding = new Label ();
			keyboardPadding.HeightRequest = 300;

			StackLayout stack = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					roleLabel,
					rolePicker,
					interferenceLabel,
					interferencePicker,
					techFoulLabel,
					techFoulEntry,
					noteLabel,
					noteEditor,
					submit,
					keyboardPadding
				}
			};

			this.Content = new ScrollView {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Content = stack
			};
		}

		void errorHandling(string d, string i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, int i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		void errorHandling(string d, bool i){
			try{
				data[d] = i;
				SaveData();
			} catch {
				errorStatus = true;
				errorString += d + " , ";
			}
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}