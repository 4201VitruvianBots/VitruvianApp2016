using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class PostMatchScoutingPage:ContentPage
	{
		ParseObject data;

		enum Choice{No, Yes};

		string errorString = "The following data was unable to be saved: ";
		bool errorStatus = false;

		public PostMatchScoutingPage (ParseObject matchData)
		{
			int choiceValue = 0;

			Label interferenceLabel = new Label {
				Text = "Did the team interferece with their alliance members/stacks?",
				TextColor = Color.Green
			};

			Picker interferencePicker = new Picker();
			for(Choice type=Choice.No; type<=Choice.Yes; type++){
				string stringValue = type.ToString();
				interferencePicker.Items.Add(stringValue);
			}

			interferencePicker.SelectedIndexChanged += (sender, args) => {
				Choice type = (Choice)interferencePicker.SelectedIndex;
				choiceValue = interferencePicker.SelectedIndex;
				string stringValue = type.ToString();
				interferencePicker.Title = stringValue;
			};

			Label fieldLabel = new Label {
				Text = "Match comments/notes:",
				TextColor = Color.Green
			};

			Editor notes = new Editor{
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
				errorHandling("interferenceCount", choiceValue);
				errorHandling("matchNotes", notes.Text);

				if(errorStatus == true){
					errorString = errorString.Remove(errorString.Length - 2); 
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: ";
				} else {
					Navigation.PushModalAsync(new PreMatchScoutingPage());
				}
				/*
				data["interferenceCount"]= choiceValue;
				data["matchNotes"] = notes.Text;
				SaveData();
				Navigation.PushModalAsync(new PreMatchScoutingPage());
				*/
			};

			Label keyboardPadding = new Label ();
			keyboardPadding.HeightRequest = 300;

			StackLayout stack = new StackLayout {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,

				Children = {
					interferenceLabel,
					interferencePicker,
					fieldLabel,
					notes,
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

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}
	}
}