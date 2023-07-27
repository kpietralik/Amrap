using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class LastStats
{
    public string Guid { get; set; }
    public PlannedExercise PlannedExercise { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    public static LastStats FromModel(LastStatsModel model, PlannedExercise plannedExercise)
    {
        if (model.PlannedExerciseGuid != plannedExercise.Guid)
            throw new Exception($"Data id missmatch: {nameof(PlannedExercise)}, model={model.PlannedExerciseGuid}, data={plannedExercise.Guid}");

        return new(plannedExercise, model.Sets, model.Reps, model.Weight, model.DropSet, model.ToFailure);
    }

    public LastStats(PlannedExercise plannedExercise, int sets, int reps, float weight, bool dropSet, bool toFailure)
    {
        // At most 1 last stats for each planned exercise
        Guid = plannedExercise.Guid;
        PlannedExercise = plannedExercise;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }
}