using System;
using Xamarin.Forms;
using Parse;

namespace VitruvianApp2016
{
	public class App : Application
	{
		public App ()
		{
			ParseClient.Initialize("df6eih4fo22hNaYhb5IB6jo5AUqE5XykXkezyAtk", "5mk9AEUsOfW8bjtNUu6fmxxvXpOgoHBifY6k8uBz");

			MainPage = new MainMenuPage ();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

