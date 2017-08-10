# Rx.Xamarin

Reactive Extensions for Xamarin

Note: This forked project is being maintained by Moby, Inc. It should primarily just be updating the package dependencies for the library, but might involve some expansion of the project in the future. 

# Usage

When you need to reference this package for your Xamarin Android project:
- Clone this repository
- Open the solution
- Switch active configuration to release
- Build
- In the project you want to add the library to, right click on references and hit edit
- Go to the .NET Assembly tab
- Click browse
- Browse to the file that you built above (it should be named Rx.Xamarin.Android.Core.dll)
- Hit Ok
- Build the project and make sure it doesn't break