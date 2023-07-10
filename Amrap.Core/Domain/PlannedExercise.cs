using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class PlannedExercise
{
    public string Guid { get; set; }
    public ExerciseType ExerciseType { get; set; }
    public LastStats LastStats { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public string Note { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    public static PlannedExercise FromModel(PlannedExerciseModel model, ExerciseType exerciseType, LastStats lastStats = null)
    {
        if (model.ExerciseTypeGuid != exerciseType.Guid)
            throw new Exception($"Data id missmatch: {nameof(PlannedExerciseModel)}, model={model.ExerciseTypeGuid}, data={exerciseType.Guid}");

        return new(model.Guid, exerciseType, model.Sets, model.Reps, model.Weight, model.Note, model.DropSet, model.ToFailure, lastStats);
    }

    private PlannedExercise(string guid, ExerciseType exerciseType, int sets, int reps, float weight, string note, bool dropSet, bool toFailure, LastStats lastStats)
    {
        Guid = guid;
        ExerciseType = exerciseType;
        Sets = sets;
        Reps = reps;
        Note = note;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
        LastStats = lastStats;
    }
}
