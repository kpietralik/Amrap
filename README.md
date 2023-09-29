![2023-08-11 23_07_52-NVIDIA GeForce Overlay DT](https://github.com/kpietralik/Amrap/assets/12815143/1419a030-ecb1-4da1-a527-d0c2f1f98241) 

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
- filtering lists by title/name value
- multiple planned exercises for each workout plan item
- charts with progress for each exercise type
- Producing bundle .abb file for Google Play Store or .apk for ad-hoc distrubution based on selected run configuration

## Debug menu 
- Version information display
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
- Delete exercise type option
- Charts with completion info for each workout plan item
- AI generated exercise type images.
- Hosting of images -> alongside app code, separate repo & package or online (e.g. Azure Storage) hosting?
- Backing up and restoring db file using native built-in mechanism (at least for Android)
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

![2023-08-11 23_07_46-NVIDIA GeForce Overlay DT](https://github.com/kpietralik/Amrap/assets/12815143/257eb197-b9fd-408e-bdd4-725f4c60aaf9)
<br/>
![2023-08-11 23_07_22-NVIDIA GeForce Overlay DT](https://github.com/kpietralik/Amrap/assets/12815143/46581c97-f62c-4b76-adb3-d88dbd0bdeb4)
<br/>
![2023-08-11 23_08_06-NVIDIA GeForce Overlay DT](https://github.com/kpietralik/Amrap/assets/12815143/179241d3-7a9b-4915-b09c-c35510d80bd6)
