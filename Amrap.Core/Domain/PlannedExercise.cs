using Amrap.Infrastructure.Db;
using SQLite;

namespace Amrap.Core.Domain;

public class PlannedExercise
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string ExerciseTypeGuid { get; set; }
    private ExerciseType _exerciseType;
    public ExerciseType ExerciseType => _exerciseType;

    private LastStats _lastStats;
    public LastStats LastStats => _lastStats;
    
    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public string Note { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PlannedExercise()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public PlannedExercise(
        string guid, ExerciseType exerciseType, int sets, int reps, float weight, 
        string note, bool dropSet, bool toFailure, LastStats? lastStats = default)
    {
        Guid = guid;
        _exerciseType = exerciseType;
        ExerciseTypeGuid = exerciseType.Guid;
        Sets = sets;
        Reps = reps;
        Note = note;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
        _lastStats = lastStats;
    }

    public void SetExerciseType(ExerciseType exerciseType)
    {
        if (exerciseType != null &&
            string.Equals(ExerciseTypeGuid, exerciseType?.Guid, StringComparison.InvariantCultureIgnoreCase))
            _exerciseType = exerciseType;
        else
            throw new Exception($"Provided {nameof(ExerciseType)} guid '{exerciseType?.Guid}' does not match expected '{ExerciseTypeGuid}'");
    }

    public void SetLastStats(LastStats lastStats)
    {
        if (lastStats != null &&
            string.Equals(Guid, lastStats.Guid, StringComparison.InvariantCultureIgnoreCase))
            _lastStats = lastStats;
        else
            throw new Exception($"Provided {nameof(LastStats)} guid '{lastStats?.Guid}' does not match expected '{Guid}'");
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddPlannedExercise(this);

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdatePlannedExercise(this);
}