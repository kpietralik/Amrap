# Amrap
"As many reps as possible"

Simple workout assistant mobile application for tracking workout plan and progress.

Written in .NET Maui using Blazor Hybrid approach

Using components from BitPlatform: https://components.bitplatform.dev/

Sample data for: Debug -> Seed Database
- exercise types 
  https://gist.githubusercontent.com/kpietralik/22cb941b0a3a9e1948a55686ef4c3067/raw/af804dfd90707a93f654f9908152bb233bd3b447/exerciseTypes.json
- workout plan
  https://gist.githubusercontent.com/kpietralik/32a930f80bfe1491b3e61e515676519d/raw/bb4a9f1568ee705535737382513936743cddfb68/workoutPlan.json

ToDo: 
CICD for Google Play Store - internal testing
Charts with progress for each exercise type
Charts completion info for each workout plan item
Placeholder image for lack of sample image cases
AI generated exercise type images.
Hosting of images -> alongside app code, separate repo & package or online (e.g. Azure Storage) hosting?
Alternative exercise.
Last X stats instead of single last stat
UI & UX improvements with focus on scalability for different devices

Core refinement 
- remove unnecessary setters
- limiting access to classes and properties
- improved error handling and messages