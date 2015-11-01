Things to NOT do:
1. Do NOT update Packages under VitruvianApp2016.Droid
>Due to the way Xamarin updates packages, updating the default 
-packages will include lines that reference Andriod API level 23 (Android 6.0). 
-Updating the packages will require you to download the SDK for Android 6.0 in order to build.
-To reduce the amount of storage memory needed to make the app, and to limit potential issues,
-just don't update the packages.