using Amrap.Core.Infrastructure;
using SQLite;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    // ToDo: made as separate entity due to planned option of multiple plannedExercises the user would pick 1 from during training session. In other words: alternative exercises.

    [SQLite.Ignore]
    public PlannedExercise PlannedExercise { get; set; }

    [Indexed]
    public DayOfWeek Day { get; set; }

    public float Priority { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public WorkoutPlanItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public WorkoutPlanItem(string guid, PlannedExercise plannedExercise, DayOfWeek day, float priority)
    {
        Guid = guid;
        PlannedExercise = plannedExercise;
        PlannedExerciseGuid = plannedExercise.Guid;
        Day = day;
        Priority = priority;
    }

    public void SetPlannedExercise(PlannedExercise plannedExercise)
    {
        if (plannedExercise != null &&
            string.Equals(PlannedExerciseGuid, plannedExercise?.Guid, StringComparison.InvariantCultureIgnoreCase))
            PlannedExercise = plannedExercise;
        else
            throw new Exception($"Provided {nameof(PlannedExercise)} guid '{plannedExercise?.Guid}' does not match expected '{PlannedExerciseGuid}'");
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddWorkoutPlanItem(this);

    public Task Upsert(DatabaseHandler databaseHandler) => databaseHandler.UpsertWorkoutPlanItem(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteWorkoutPlanItem(Guid);

    public int GetSets() => PlannedExercise.LastStats?.Sets ?? PlannedExercise.Sets;

    public int GetReps() => PlannedExercise.LastStats?.Reps ?? PlannedExercise.Reps;

    public float GetWeight() => PlannedExercise.LastStats?.Weight ?? PlannedExercise.Weight;

    public bool GetDropSet() => PlannedExercise.LastStats?.DropSet ?? PlannedExercise.DropSet;

    public bool GetToFailure() => PlannedExercise.LastStats?.ToFailure ?? PlannedExercise.ToFailure;
}