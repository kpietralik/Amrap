using Amrap.Core.Models;
using Amrap.Infrastructure.Db;
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
    private ExerciseType _exerciseType;
    public ExerciseType ExerciseType => _exerciseType;

    public DateTimeOffset Time { get; set; }
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

    public CompletedExercise(ExerciseType exerciseType, DateTimeOffset time, int sets, int reps, float weight, bool dropSet = false, bool toFailure = false)
    {
        _exerciseType = exerciseType;
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
            _exerciseType = exerciseType;
        else
            throw new Exception($"Provided {nameof(ExerciseType)} guid '{exerciseType?.Guid}' does not match expected '{ExerciseTypeGuid}'");
    }

    public async Task SaveCompletedExercise(DatabaseHandler databaseHandler, string plannedExerciseGuid)
    {
        await databaseHandler.AddExercise(this);

        var lastStats = new LastStatsModel(
            plannedExerciseGuid,
            Sets,
            Reps,
            Weight,
            DropSet,
            ToFailure);

        await databaseHandler.SetLastStats(lastStats);
    }

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteCompletedExercise(Id);
}