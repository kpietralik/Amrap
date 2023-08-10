using Amrap.Core.Infrastructure;
using SQLite;

namespace Amrap.Core.Domain;

public class CompletedExercise
{
    [AutoIncrement]
    [PrimaryKey]
    public int Id { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string ExerciseTypeGuid { get; set; }

    [SQLite.Ignore]
    public ExerciseType ExerciseType { get; set; }

    public DateTime Time { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public CompletedExercise()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public CompletedExercise(ExerciseType exerciseType, DateTime time, int sets, int reps, float weight, bool dropSet = false, bool toFailure = false)
    {
        ExerciseType = exerciseType;
        ExerciseTypeGuid = exerciseType.Guid;
        Time = time;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }

    public void SetExerciseType(ExerciseType exerciseType)
    {
        if (exerciseType != null &&
            string.Equals(ExerciseTypeGuid, exerciseType?.Guid, StringComparison.InvariantCultureIgnoreCase))
            ExerciseType = exerciseType;
        else
            throw new Exception($"Provided {nameof(ExerciseType)} guid '{exerciseType?.Guid}' does not match expected '{ExerciseTypeGuid}'");
    }

    public async Task SaveCompletedExercise(DatabaseHandler databaseHandler, PlannedExercise plannedExercise)
    {
        await databaseHandler.AddExercise(this);

        var lastStats = new LastStats(
            plannedExercise,
            Sets,
            Reps,
            Weight,
            DropSet,
            ToFailure);

        await lastStats.Save(databaseHandler);
    }

    public Task ImportCompletedExercise(DatabaseHandler databaseHandler) => databaseHandler.UpsertExercise(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteCompletedExercise(Id);
}