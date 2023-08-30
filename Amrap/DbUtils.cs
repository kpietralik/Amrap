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
        var compltedExercises = await CompletedExercise.ReadCompletedExercies(databaseHandler);

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

        var json = JsonSerializer.Serialize(workoutPlan, _jsonOptions);

        using var stream = new MemoryStream(Encoding.Default.GetBytes(json));

        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, default);

        return fileSaverResult.IsSuccessful;
    }

    internal static async Task<bool> ImportWorkoutPlan(DatabaseHandler databaseHandler)
    {
        var workoutPlan = await PickAndReadJsonFile<WorkoutPlanItem>(JsonPickOptions());

        if (workoutPlan == null)
            return false;

        await ImportWorkoutPlan(databaseHandler, workoutPlan);

        return true;
    }

    internal static async Task SeedData(DatabaseHandler databaseHandler, Uri exerciseTypesUrl, Uri workoutPlanUrl)
    {
        using var client = new HttpClient();

        var exerciseTypes = await client.GetFromJsonAsync<IList<ExerciseType>>(exerciseTypesUrl);
        var workoutPlan = await client.GetFromJsonAsync<IList<WorkoutPlanItem>>(workoutPlanUrl);

        await databaseHandler.SeedExerciseTypes(exerciseTypes);

        await ImportWorkoutPlan(databaseHandler, workoutPlan);
    }

    private static async Task<ICollection<T>> PickAndReadJsonFile<T>(PickOptions options)
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

    private static async Task ImportWorkoutPlan(DatabaseHandler databaseHandler, ICollection<WorkoutPlanItem> workoutPlan)
    {
        foreach (var workoutPlanItem in workoutPlan)
        {
            foreach (var plannedExercise in workoutPlanItem.PlannedExercises)
            {
                await plannedExercise.Add(databaseHandler);

                if (plannedExercise.LastStats != null)
                    await plannedExercise.LastStats.Import(databaseHandler);
            }

            await workoutPlanItem.Upsert(databaseHandler);
        }
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
