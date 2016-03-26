using System;
using System.IO;
using Xamarin.Media;
using Xamarin.Forms;
using Parse;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using FFImageLoading.Forms;

namespace VitruvianApp2016
{
	public class ImagePopupPage:PopupPage
	{
		ParseObject data;
		MediaFile robotImageFile;
		CachedImage robotImage = new CachedImage ();

		Grid layoutGrid = new Grid (){
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,

			ColumnDefinitions = {
				new ColumnDefinition{ Width = GridLength.Auto },
				new ColumnDefinition{ Width = new GridLength(1, GridUnitType.Star) },	
			},
			RowDefinitions = {		
				new RowDefinition{ Height = new GridLength(1, GridUnitType.Star) },		// robotImage
				new RowDefinition{ Height = GridLength.Auto },							// backBtn
			}
		};
		public ImagePopupPage (CachedImage robotImage)
		{
			robotImage.Aspect = Aspect.Fill;
			//Back Button
			Button backBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				popUpReturn();
			};

			layoutGrid.Children.Add(robotImage, 0, 2, 0, 1);
			layoutGrid.Children.Add(backBtn, 0, 2, 1, 2);

			this.Content = layoutGrid;
		}

		public ImagePopupPage (CachedImage robotImage, ParseObject teamData)
		{
			data = teamData;
			robotImage.Aspect = Aspect.Fill;

			//Back Button
			Button retakeImageBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Retake Image",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			retakeImageBtn.Clicked += (object sender, EventArgs e) => {
				OpenImagePicker();
			};

			//Back Button
			Button backBtn = new Button () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Text = "Back",
				TextColor = Color.Green,
				BackgroundColor = Color.Black
			};
			backBtn.Clicked += (object sender, EventArgs e) => {
				popUpReturn();
			};

			StackLayout navigationBtns = new StackLayout () {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				BackgroundColor = Color.Green,
				Padding = 5,

				Children = {
					retakeImageBtn,
					backBtn
				}
			};

			layoutGrid.Children.Add(robotImage, 0, 2, 0, 1);
			layoutGrid.Children.Add(navigationBtns, 0, 2, 1, 2);

			this.Content = layoutGrid;
		}

		async void popUpReturn(){
			await Task.Yield ();
			await PopupNavigation.PopAsync ();
		}

		public byte[] ImageToBinary(string imagePath)
		{
			FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[fileStream.Length];
			fileStream.Read(buffer, 0, (int)fileStream.Length);
			fileStream.Close();
			return buffer;
		}

		async void OpenImagePicker(){
			//It works? Don't use gallery
			var robotImagePicker = new MediaPicker(Forms.Context);

			await robotImagePicker.TakePhotoAsync(new StoreCameraMediaOptions {
				Name = data["teamNumber"].ToString() + ".jpg",
				Directory = "Robot Images"
			}).ContinueWith(t=>{
				robotImageFile = t.Result;
				Console.WriteLine("Robot Image Path: " + robotImageFile.Path);
			},TaskScheduler.FromCurrentSynchronizationContext());
			robotImage.Source = robotImageFile.Path;
			try{
				ParseFile image = new ParseFile(data["teamNumber"].ToString()+".jpg", ImageToBinary(robotImageFile.Path));

				data["robotImage"] = image;
				await data.SaveAsync();
				popUpReturn();
			}
			catch{
				Console.WriteLine ("Image Save Error");
			}
		}
	}
}

