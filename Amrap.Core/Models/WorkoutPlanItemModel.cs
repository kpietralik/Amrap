using SQLite;

namespace Amrap.Core.Models;

public class WorkoutPlanItemModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // TODO Turn into relationship table with multiple values
    //   [Indexed]
    //   public List<int> PlannedExercies { get; set; }

    [Indexed]
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
