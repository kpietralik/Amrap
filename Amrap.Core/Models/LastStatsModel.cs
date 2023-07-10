using SQLite;

namespace Amrap.Core.Models;

public class LastStatsModel
{
    [PrimaryKey]
    public string Guid { get; set; }

    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public bool DropSet { get; set; }

    // For SQLite
    public LastStatsModel() { }

    public LastStatsModel(string plannedExerciseGuid, int sets, int reps, float weight, bool dropSet)
    {
        // At most 1 last stats for each planned exercise
        Guid = plannedExerciseGuid;
        PlannedExerciseGuid = plannedExerciseGuid;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
    }
}
