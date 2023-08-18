using Amrap.Core.Infrastructure;
using SQLite;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    [PrimaryKey]
    public string Guid { get; set; }

    [Indexed]
    public DayOfWeek Day { get; set; }

    public float Priority { get; set; }

    public string Title { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    /// <remarks>
    /// SQLite only
    /// </remarks>
    // Left only to apply Migration1
    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    [SQLite.Ignore]
    public IList<PlannedExercise> PlannedExercises { get; set; } = new List<PlannedExercise>();

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public WorkoutPlanItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public WorkoutPlanItem(string guid, DayOfWeek day,  float priority, string title)
    {
        Guid = guid;
        Day = day;
        Priority = priority;
        Title = title;
    }

    public void SetPlannedExercises(IList<PlannedExercise> plannedExercises)
    {
        //if (plannedExercises?.Any() == false)
        //    throw new Exception($"Provided {nameof(plannedExercises)} list is null or empty");

        foreach(var plannedExersise in plannedExercises)
        {
            if (!string.Equals(plannedExersise.WorkoutPlanItemGuid, Guid, StringComparison.InvariantCultureIgnoreCase))
                throw new Exception($"Provided {nameof(Domain.PlannedExercise)} guid '{plannedExersise?.WorkoutPlanItemGuid}' does not match expected '{Guid}'");

            PlannedExercises.Add(plannedExersise);
        }
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddWorkoutPlanItem(this);

    public Task Upsert(DatabaseHandler databaseHandler) => databaseHandler.UpsertWorkoutPlanItem(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteWorkoutPlanItem(this);

    public static async Task<WorkoutPlanItem> GetWorkoutPlanItem(DatabaseHandler databaseHandler, string guid)
    {
        var workoutPlanItem = await databaseHandler.GetWorkoutPlanItem(guid);

        return workoutPlanItem;
    }
}