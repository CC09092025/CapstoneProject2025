# PantryPal

## About PantryPal

PantryPal is a cross-platform app designed to make tracking your pantry simple. It helps reduce food waste and makes cooking more enjoyable. Without PantryPal, you’d need to constantly check which items are nearing expiration, past their best-before date, or have gone bad — a tedious and frustrating process, especially when you discover an ingredient you planned to use has already expired.

## Main Features

* **Pantry Inventory:** Maintain a complete list of all products and ingredients in your pantry or kitchen.
* **Expiration Tracking:** Track expiration and best-before dates for every item.
* **Notifications:** Receive alerts when items are approaching expiration or have already expired.

## Building and Running
### 1. Prerequisites
Make sure you have the following installed
* .NET 9 SDK (or later)
* Visual Studio 2022 (or later) with the following workloads installed:
  * .NET Multi-platform App UI development
  * Mobile development with .NET (for Android/iOS)
  * Optional: Desktop development with C# (for Windows/macOS)
* Android Emulator (via Visual Studio) or physical device
* Xcode (macOS only, required for iOS/iPadOS builds)

### 2. Clone the Repository
```
git clone https://github.com/YourUsername/PantryPal.git
cd PantryPal
```
### 3. Restore NuGet Packages
```
dotnet restore
```
### 4. Build the Project
> [!note]
> For whatever reason dotnet believes there to be a duplicate of the appicon.svg, therefor it refuses to build despite there not appearing to be said duplicate of said file. Visual Studio builds without issue, though.
* Windows/macOS Desktop
```
dotnet build -f net9.0-windows10.0.19041.0
dotnet build -f net9.0-maccatalyst
```
* Android
```
dotnet build -f net9.0-android
```
* iOS/iPad OS (macOS required)
```
dotnet build -f net9.0-ios
```
### 5. Run the App
* Windows/macOS Desktop
```
dotnet run -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-maccatalyst
```
* Android
```
dotnet run -f net9.0-android
```
* iOS/iPad OS
```
dotnet run -f net9.0-ios
```

## Challenges Encountered

### Unit Testing

Despite multiple attempts, I was unable to get unit testing working as intended. The tests either:

* Complained about inaccessible members due to protection levels, or
* Attempted to initialize Windows-specific components unavailable in the test context.

I couldn’t find an easy or reliable solution, so the only testing performed was **manual eyeball testing** by running the application, which behaved as expected.

### CI Actions

Given the inability to perform unit testing, testing via CI was also affected. My plan for the CI workflow was:

1. Checkout the repository
2. Set up the .NET 9 SDK
3. Install MAUI workloads
4. Restore dependencies
5. Build the Windows target

However, all attempts failed at the **Windows build step**, with an error related to installing the runtime pack for `Microsoft.NETCore.App.Runtime.win-x64`. This issue created a persistent impasse, preventing the workflow from completing successfully despite troubleshooting multiple errors.
