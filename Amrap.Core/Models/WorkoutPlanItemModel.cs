using SQLite;

namespace Amrap.Core.Models;

public class WorkoutPlanItemModel
{
    [PrimaryKey]
    public string Guid { get; set; }

    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    [Indexed]
    public DayOfTheWeek Day { get; set; }

    // For SQLite
    public WorkoutPlanItemModel() { }

    public WorkoutPlanItemModel(string guid, DayOfTheWeek day, PlannedExerciseModel plannedExercise)
    {
        Guid = guid;
        Day = day;
        PlannedExerciseGuid = plannedExercise.Guid;
    }
}
