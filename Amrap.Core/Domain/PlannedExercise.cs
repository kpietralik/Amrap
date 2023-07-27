using Amrap.Core.Models;
using Amrap.Infrastructure.Db;

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

    public static PlannedExercise FromModel(PlannedExerciseModel model, ExerciseType exerciseType, LastStats? lastStats = default)
    {
        if (model.ExerciseTypeGuid != exerciseType.Guid)
            throw new Exception($"Data id missmatch: {nameof(PlannedExerciseModel)}, model={model.ExerciseTypeGuid}, data={exerciseType.Guid}");

        return new(model.Guid, exerciseType, model.Sets, model.Reps, model.Weight, model.Note, model.DropSet, model.ToFailure, lastStats);
    }

    public PlannedExercise(string guid, ExerciseType exerciseType, int sets, int reps, float weight, string note, bool dropSet, bool toFailure, LastStats? lastStats = default)
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

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddPlannedExercise(
            new PlannedExerciseModel(
                Guid,
                ExerciseType.Guid,
                Sets,
                Reps,
                Weight,
                Note,
                DropSet,
                ToFailure));

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdatePlannedExercise(
            new PlannedExerciseModel(
                Guid,
                ExerciseType.Guid,
                Sets,
                Reps,
                Weight,
                Note,
                DropSet,
                ToFailure));

    public static async Task<IList<PlannedExercise>> GetPlannedExercises(DatabaseHandler databaseHandler, ExerciseType exerciseType)
    {
        var plannedExerciseModels = await databaseHandler.GetPlannedExercises();

        var plannedExercises = new List<PlannedExercise>();
        foreach (var plannedExercise in plannedExerciseModels)
        {
            plannedExercises.Add(FromModel(plannedExercise, exerciseType));
        }

        return plannedExercises;
    }
}
