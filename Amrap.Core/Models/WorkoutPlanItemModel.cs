using SQLite;

namespace Amrap.Core.Models;

public class WorkoutPlanItemModel
{
    [PrimaryKey]
    public string Guid { get; set; }

    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    [Indexed]
    public DayOfWeek Day { get; set; }

    // For SQLite
    public WorkoutPlanItemModel() { }

    public WorkoutPlanItemModel(string guid, DayOfWeek day, string plannedExerciseGuid)
    {
        Guid = guid;
        Day = day;
        PlannedExerciseGuid = plannedExerciseGuid;
    }
}
