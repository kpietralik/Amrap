# Amrap
## Introduction
Amrap = "As many reps as possible"

Simple offline workout assistant mobile application for tracking workout plan and progress.

- Written in .NET MAUI with Blazor Hybrid approach
- SQLite used through [SQLite-net](https://github.com/praeclarum/sqlite-net)
- UI components from [BitPlatform](https://components.bitplatform.dev/)

All data is stored locally. 

## Working Features
- seeding database with sample exercise types and workout plan data
- creating/editing exercise type from UI
- creating/edititg/deleting workout plan item from UI
- presenting workout plan
- filtering workout plan based on day of week and completed status (current day)
- multiple planned exercises for each workout plan item
- charts with progress for each exercise type

## Debug menu 
- `Recreate Database keeping achievements` removes all db data except completed exercises
- `Recreate Database deleting all data` removes all db data
- `Export Completed Exercises` - exports completed exercises
- `Import Completed Exercises` - source exercise type must be present in the db for import to work
- `Export Exercise Types`
- `Import Exercise Types`
- `Export Workout plan`
- `Import Workout plan`
- `Seed Database` uses provided links to seed exercise types and workout plan. Clearing database before that operation is advised.

Sample data for `Seed Database` functionality:
- exercise types 
  https://gist.githubusercontent.com/kpietralik/22cb941b0a3a9e1948a55686ef4c3067/raw/af804dfd90707a93f654f9908152bb233bd3b447/exerciseTypes.json
- workout plan
  https://gist.githubusercontent.com/kpietralik/32a930f80bfe1491b3e61e515676519d/raw/bb4a9f1568ee705535737382513936743cddfb68/workoutPlan.json

## ToDo:
### Features
- Order exercise types by name
- Filter exercise types by name
- Delete exercise type option
- Charts with completion info for each workout plan item
- Placeholder image for lack of sample image cases
- AI generated exercise type images.
- Hosting of images -> alongside app code, separate repo & package or online (e.g. Azure Storage) hosting?
- Backing up and restoring db file using native built-in mechanism (at least for Android)
- CICD for Google Play Store - internal testing
- UI & UX improvements with focus on scalability for different devices
- Continous improvement to README.md

### Code refinement 
- unit tests (how to handle SQLite? Is it sensible to mock it?)
- remove unnecessary setters
- limiting access to classes and properties
- improved error handling and messages
- log viewing (local or online)

## Getting started
Prequisites:
- Official [.NET MAUI installation steps](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?tabs=vswin)
- .NET 7 SDK 
- Visual Studio 2022 (developed using version 17.7.0)
- Installed WASM workload **for .NET 7**: `dotnet workload install wasm-tools**-net7**`
- Created Android emulator device - app developed using Pixel 5 virtual device with API 31 (Android 12.0)
- Enabled Hyper-V and HAXM [Link](https://learn.microsoft.com/en-gb/xamarin/android/get-started/installation/android-emulator/hardware-acceleration?tabs=vswin)

Troubleshooting
- Following official docs will likely cause you to install `wasm-tools` for **.NET 8 preview** which is may not be compatible with this project.
- If message about missing `wasm-tools` workload appears, try checking installed version:

  `dotnet workload list`
   
   Then try to clean up preview workload and install correct version only.
