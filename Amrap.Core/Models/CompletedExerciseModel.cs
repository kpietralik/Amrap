using SQLite;

namespace Amrap.Core.Models;

public class CompletedExerciseModel
{
    [AutoIncrement]
    [PrimaryKey]
    public int Id { get; set; }

    [Indexed]
    public string ExerciseTypeGuid { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    // For SQLite
    public CompletedExerciseModel() { }

    public CompletedExerciseModel(
        string exerciseTypeGuid, DateTimeOffset time, int sets, int reps, float weight, bool dropSet = false, bool toFailure = false)
    {
        ExerciseTypeGuid = exerciseTypeGuid;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }
}
