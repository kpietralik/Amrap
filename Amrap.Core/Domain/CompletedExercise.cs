using Amrap.Core.Models;
using Amrap.Infrastructure.Db;

namespace Amrap.Core.Domain;

public class CompletedExercise
{
    public int Id { get; set; } 

    public ExerciseType ExerciseType { get; set; }

    public DateTimeOffset Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    public static CompletedExercise FromModel(CompletedExerciseModel model, ExerciseType exerciseType)
    {
        if (model.ExerciseTypeGuid != exerciseType.Guid.ToString())
            throw new Exception(
                $"Data id missmatch for {nameof(CompletedExercise)} {nameof(ExerciseType)}: " +
                $"{nameof(CompletedExerciseModel)}, model={model.ExerciseTypeGuid}, data={exerciseType.Guid}");

        return new CompletedExercise(model.Id, exerciseType, model.Time, model.Sets, model.Reps, model.Weight, model.DropSet, model.ToFailure);
    }

    public CompletedExerciseModel ToModel() =>
        new(ExerciseType.Guid, Time, Sets, Reps, Weight, DropSet, ToFailure);

    public CompletedExercise(ExerciseType exerciseType, DateTimeOffset time, int sets, int reps, float weight, bool dropSet = false, bool toFailure = false)
    {
        ExerciseType = exerciseType;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }

    private CompletedExercise(int id, ExerciseType exerciseType, DateTimeOffset time, int sets, int reps, float weight, bool dropSet = false, bool toFailure = false)
    {
        Id = id;
        ExerciseType = exerciseType;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }

    public async Task SaveCompletedExercise(DatabaseHandler databaseHandler, string plannedExerciseGuid)
    {
        await databaseHandler.AddExercise(this.ToModel());

        var lastStats = new LastStatsModel(
            plannedExerciseGuid,
            Sets,
            Reps,
            Weight,
            DropSet,
            ToFailure);

        await databaseHandler.SetLastStats(lastStats);
    }
}
