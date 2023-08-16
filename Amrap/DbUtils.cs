using Amrap.Core;
using Amrap.Core.Domain;
using Amrap.Core.Infrastructure;
using CommunityToolkit.Maui.Storage;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Amrap;

internal static class DbUtils
{
    private static JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    internal static async Task<bool> ExportCompletedExercises(DatabaseHandler databaseHandler, string fileName = "completedExercises.json")
    {
        var completedExerciseReader = new CompletedExerciseReader(databaseHandler);
        var compltedExercises = await completedExerciseReader.ReadCompletedExercies();

        var json = JsonSerializer.Serialize(compltedExercises, _jsonOptions);
        
        using var stream = new MemoryStream(Encoding.Default.GetBytes(json));

        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, default);

        return fileSaverResult.IsSuccessful;
    }

    internal static async Task<bool> ImportCompletedExercises(DatabaseHandler databaseHandler)
    {
        var exerciseTypes = await databaseHandler.GetExerciseTypes();

        var completedExercises = await PickAndReadJsonFile<CompletedExercise>(JsonPickOptions());

        if (completedExercises == null)
            return false;

        foreach (var completedExercise in completedExercises)
        {
            completedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == completedExercise.ExerciseTypeGuid));

            await completedExercise.ImportCompletedExercise(databaseHandler);
        }

        return true;
    }

    internal static async Task<bool> ExportExerciseTypes(DatabaseHandler databaseHandler, string fileName = "exerciseTypes.json")
    {
        var exerciseTypes = await databaseHandler.GetExerciseTypes();

        var json = JsonSerializer.Serialize(exerciseTypes, _jsonOptions);

        using var stream = new MemoryStream(Encoding.Default.GetBytes(json));

        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, default);

        return fileSaverResult.IsSuccessful;
    }

    internal static async Task<bool> ImportExerciseTypes(DatabaseHandler databaseHandler)
    {
        var exerciseTypes = await PickAndReadJsonFile<ExerciseType>(JsonPickOptions());

        if (exerciseTypes == null)
            return false;

        foreach (var exerciseType in exerciseTypes)
            await exerciseType.Upsert(databaseHandler);

        return true;
    }

    internal static async Task<bool> ExportWorkoutPlan(DatabaseHandler databaseHandler, string fileName = "workoutPlan.json")
    {
        var workoutPlan = await databaseHandler.GetWorkoutPlan();

        // ToDo: test serialization
        var json = JsonSerializer.Serialize(workoutPlan, _jsonOptions);

        using var stream = new MemoryStream(Encoding.Default.GetBytes(json));

        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, default);

        return fileSaverResult.IsSuccessful;
    }

    internal static async Task<bool> ImportWorkoutPlan(DatabaseHandler databaseHandler)
    {
        // ToDo: test deserialization
        throw new NotImplementedException();

        //var workoutPlan = await PickAndReadJsonFile<WorkoutPlanItem>(JsonPickOptions());

        //if (workoutPlan == null)
        //    return false;

        //foreach (var workoutPlanItem in workoutPlan)
        //{
        //    await workoutPlanItem.PlannedExercise.Upsert(databaseHandler);

        //    if (workoutPlanItem.PlannedExercise.LastStats != null)
        //        await workoutPlanItem.PlannedExercise.LastStats.Save(databaseHandler);
            
        //    await workoutPlanItem.Upsert(databaseHandler);
        //}

        //return true;
    }

    public static async Task<ICollection<T>> PickAndReadJsonFile<T>(PickOptions options)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var deserialized = await JsonSerializer.DeserializeAsync<ICollection<T>>(stream, _jsonOptions);

                    return deserialized;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
    }

    internal static async Task SeedData(DatabaseHandler databaseHandler, Uri exerciseTypesUrl, Uri workoutPlanUrl)
    {
        // ToDo: test seeding data
        throw new NotImplementedException();

        //using var client = new HttpClient();

        //var exerciseTypes = await client.GetFromJsonAsync<IList<ExerciseType>>(exerciseTypesUrl);
        //var workoutPlan = await client.GetFromJsonAsync<IList<WorkoutPlanItem>>(workoutPlanUrl);

        //await databaseHandler.SeedExerciseTypes(exerciseTypes);

        //var plannedExercises = workoutPlan.Select(x => x.PlannedExercise).Distinct(new PlannedExerciseEqualityComparer());
        //foreach (var plannedExercise in plannedExercises)
        //{
        //    plannedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == plannedExercise.ExerciseTypeGuid));
        //    await plannedExercise.Add(databaseHandler);
        //}

        //foreach (var workoutPlanItem in workoutPlan)
        //{
        //    workoutPlanItem.SetPlannedExercise(plannedExercises.Single(x => x.Guid == workoutPlanItem.PlannedExerciseGuid));
        //    await workoutPlanItem.Add(databaseHandler);
        //}
    }

    private static PickOptions JsonPickOptions()
    {
        var jsonFileType = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "application/json" } }, // MIME type
                    { DevicePlatform.WinUI, new[] { ".json" } }, // file extension
                    //{ DevicePlatform.iOS, new[] { "<fake> public.my.comic.extension" } }, // UTType values
                    //{ DevicePlatform.macOS, new[] { "<fake> cbr"} }, // UTType values
                });

        PickOptions pickOptions = new()
        {
            PickerTitle = "Please select json file",
            FileTypes = jsonFileType,
        };

        return pickOptions;
    }
}
