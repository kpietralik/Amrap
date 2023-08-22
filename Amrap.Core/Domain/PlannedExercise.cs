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

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string WorkoutPlanItemGuid { get; set; }

    public int Sets { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public string Note { get; set; }
    public bool DropSet { get; set; }
    public bool ToFailure { get; set; }

    private ExerciseStat? LastStatsFirstEntry =>
        LastStats?.ExerciseStats?.FirstOrDefault();

    public int GetSets() => LastStatsFirstEntry?.Sets ?? Sets;

    public int GetReps() => LastStatsFirstEntry?.Reps ?? Reps;

    public float GetWeight() => LastStatsFirstEntry?.Weight ?? Weight;

    public bool GetDropSet() => LastStatsFirstEntry?.DropSet ?? DropSet;

    public bool GetToFailure() => LastStatsFirstEntry?.ToFailure ?? ToFailure;

    public string GetDropSet(Func<bool, string> boolStringFormatFunc) => boolStringFormatFunc(LastStatsFirstEntry?.DropSet ?? DropSet);

    public string GetToFailure(Func<bool, string> boolStringFormatFunc) => boolStringFormatFunc(LastStatsFirstEntry?.ToFailure ?? ToFailure);

    public IEnumerable<int> GetSetsArray()
    {
        if (LastStatsFirstEntry?.Sets == null)
            return new int[] { Sets };

        return LastStats.ExerciseStats.Select(x => x.Sets);
    }

    public IEnumerable<int> GetRepsArray()
    {
        if (LastStatsFirstEntry?.Reps == null)
            return new int[] { Reps };

        return LastStats.ExerciseStats.Select(x => x.Reps);
    }

    public IEnumerable<float> GetWeightArray()
    {
        if (LastStatsFirstEntry?.Weight == null)
            return new float[] { Weight };

        return LastStats.ExerciseStats.Select(x => x.Weight);
    }

    public IEnumerable<string> GetDropSetArray(Func<bool, string> boolStringFormatFunc)
    {
        if (LastStatsFirstEntry?.DropSet == null)
            return new string[] { boolStringFormatFunc(DropSet) };

        return LastStats.ExerciseStats.Select(x => boolStringFormatFunc(x.DropSet));
    }

    public IEnumerable<string> GetToFailureArray(Func<bool, string> boolStringFormatFunc)
    {
        if (LastStatsFirstEntry?.ToFailure == null)
            return new string[] { boolStringFormatFunc(ToFailure) };

        return LastStats.ExerciseStats.Select(x => boolStringFormatFunc(x.ToFailure));
    }

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public PlannedExercise()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public PlannedExercise(
        string guid, ExerciseType exerciseType, WorkoutPlanItem workoutPlanItem, int sets, int reps, float weight,
        string note, bool dropSet, bool toFailure, LastStats? lastStats = default)
    {
        Guid = guid;
        ExerciseType = exerciseType;
        ExerciseTypeGuid = exerciseType.Guid;
        WorkoutPlanItemGuid = workoutPlanItem.Guid;
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

    public static async Task<PlannedExercise> GetPlannedExercise(DatabaseHandler databaseHandler, string guid, WorkoutPlanItem workoutPlanItem)
    {
        var plannedExercise = await databaseHandler.GetPlannedExercise(guid);

        var exerciseType = await databaseHandler.GetExerciseType(plannedExercise.ExerciseTypeGuid);

        plannedExercise.SetExerciseType(exerciseType);

        return plannedExercise;
    }

    public static async Task<IList<PlannedExercise>> GetPlannedExercisesForWorkoutPlanItem(DatabaseHandler databaseHandler, WorkoutPlanItem workoutPlanItem)
    {
        // ToDo: Or pass as parameter. Or do this task in databaseHandler.
        var exerciseTypes = await databaseHandler.GetExerciseTypes();

        var plannedExercise = await databaseHandler.GetPlannedExercisesForWorkoutPlanItem(workoutPlanItem, exerciseTypes);

        return plannedExercise;
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddPlannedExercise(this);

    public Task Upsert(DatabaseHandler databaseHandler) => databaseHandler.UpsertPlannedExercise(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeletePlannedExercise(this);

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
