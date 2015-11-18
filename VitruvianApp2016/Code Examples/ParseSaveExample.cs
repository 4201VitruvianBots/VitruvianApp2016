using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class ParseSaveExample:ContentPage
	{
		ParseObject data;

		public ParseSaveExample (ParseObject obj)
		{
			string errorString = "The following data was unable to be saved: ";
			bool error = false;
			data = obj;

			Entry teamNameEntry = new Entry () {
				Text = "Enter Team Name"
			};

			Button updateBtn = new Button () {
				Text = "Update Data"
			};
			updateBtn.Clicked += (object sender, EventArgs e) => {
				// For each piece of data you want to save, use a try/catch for user feedback if data was unable to be saved
				try{
					// Do not upload data if data != default text
					if(teamNameEntry.Text != "Enter Team Name"){
						data["teamName"] = teamNameEntry.Text;
						SaveData ();
					}
				} catch {
					error = true;
					errorString += "teamName, ";
				}

				// If an error is encountered, a display alert will appear
				if(error == true){
					errorString = errorString.Remove(errorString.Length - 2); // Remove the " ," from the last element and store it as the string, tehn display it
					DisplayAlert("Error:", errorString, "OK");
					errorString = "The following data was unable to be saved: "; // Reset the Error String
				} else {
					//Navigation.PushModalAsync (new Page());
				}
			};
		}

		async void SaveData(){
			Console.WriteLine ("Saving...");
			await data.SaveAsync ();
			Console.WriteLine ("Done Saving");
		}

	}
}

