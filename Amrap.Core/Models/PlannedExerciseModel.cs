using SQLite;

namespace Amrap.Core.Models;

public class PlannedExerciseModel
{
    [PrimaryKey]
    public string Guid { get; set; }

    [Indexed]
    public string ExerciseTypeGuid { get; set; }

    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public string Note { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    // For SQLite
    public PlannedExerciseModel() { }

    public PlannedExerciseModel(string guid, ExerciseTypeModel exerciseType, int sets, int reps, float weight, string note = default, bool dropSet = false, bool toFailure = false)
    {
        Guid = guid;
        ExerciseTypeGuid = exerciseType.Guid;
        Sets = sets;
        Reps = reps;
        Note = note;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }
}
