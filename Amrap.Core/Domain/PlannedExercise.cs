using Amrap.Core.Infrastructure;
using SQLite;
using System.Diagnostics.CodeAnalysis;

namespace Amrap.Core.Domain;

public class PlannedExercise : IEqualityComparer<PlannedExercise>
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string ExerciseTypeGuid { get; set; }

    [SQLite.Ignore]
    public ExerciseType ExerciseType { get; set; }

    [SQLite.Ignore]
    public LastStats LastStats { get; set; }

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
        ExerciseType = exerciseType;
        ExerciseTypeGuid = exerciseType.Guid;
        Sets = sets;
        Reps = reps;
        Note = note;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
        LastStats = lastStats;
    }

    public void SetExerciseType(ExerciseType exerciseType)
    {
        if (exerciseType != null &&
            string.Equals(ExerciseTypeGuid, exerciseType?.Guid, StringComparison.InvariantCultureIgnoreCase))
            ExerciseType = exerciseType;
        else
            throw new Exception($"Provided {nameof(ExerciseType)} guid '{exerciseType?.Guid}' does not match expected '{ExerciseTypeGuid}'");
    }

    public void SetLastStats(LastStats lastStats)
    {
        if (lastStats != null &&
            string.Equals(Guid, lastStats.Guid, StringComparison.InvariantCultureIgnoreCase))
            LastStats = lastStats;
        else
            throw new Exception($"Provided {nameof(LastStats)} guid '{lastStats?.Guid}' does not match expected '{Guid}'");
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddPlannedExercise(this);

    public Task Upsert(DatabaseHandler databaseHandler) => databaseHandler.UpsertPlannedExercise(this);

    public bool Equals(PlannedExercise? x, PlannedExercise? y)
    {
        if (x == null && y == null)
            return true;

        if (x == null && y != null)
            return false;

        if (x != null && y == null)
            return false;

        return string.Equals(x?.Guid, y?.Guid, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode([DisallowNull] PlannedExercise obj)
    {
        return this.GetHashCode();
    }
}

public class PlannedExerciseEqualityComparer : IEqualityComparer<PlannedExercise>
{
    public bool Equals(PlannedExercise? x, PlannedExercise? y)
    {
        if (x == null && y == null)
            return true;

        if (ReferenceEquals(x, y)) 
            return true;

        if (x is null || y is null)
            return false;

        return string.Equals(x?.Guid, y?.Guid, StringComparison.InvariantCultureIgnoreCase);
    }

    public int GetHashCode([DisallowNull] PlannedExercise obj)
    {
        return this.GetHashCode();
    }
}
