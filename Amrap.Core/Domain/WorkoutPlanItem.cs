using Amrap.Infrastructure.Db;
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
    private PlannedExercise _plannedExercise;
    public PlannedExercise PlannedExercise => _plannedExercise;

    [Indexed]
    public DayOfWeek Day { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public WorkoutPlanItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public WorkoutPlanItem(string guid, PlannedExercise plannedExercise, DayOfWeek day)
    {
        Guid = guid;
        _plannedExercise = plannedExercise;
        PlannedExerciseGuid = plannedExercise.Guid;
        Day = day;
    }

    public void SetPlannedExercise(PlannedExercise plannedExercise)
    {
        if (plannedExercise != null &&
            string.Equals(PlannedExerciseGuid, plannedExercise?.Guid, StringComparison.InvariantCultureIgnoreCase))
            _plannedExercise = plannedExercise;
        else
            throw new Exception(                $"Provided {nameof(PlannedExercise)} guid '{plannedExercise?.Guid}' does not match expected '{PlannedExerciseGuid}'");
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddWorkoutPlanItem(this);

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdateWorkoutPlanItem(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteWorkoutPlanItem(Guid);

    public int GetSets() => PlannedExercise.LastStats?.Sets ?? PlannedExercise.Sets;

    public int GetReps() => PlannedExercise.LastStats?.Reps ?? PlannedExercise.Reps;

    public float GetWeight() => PlannedExercise.LastStats?.Weight ?? PlannedExercise.Weight;

    public bool GetDropSet() => PlannedExercise.LastStats?.DropSet ?? PlannedExercise.DropSet;

    public bool GetToFailure() => PlannedExercise.LastStats?.ToFailure ?? PlannedExercise.ToFailure;
}