using Amrap.Core.Infrastructure;
using SQLite;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    // ToDo: made as separate entity due to planned option of multiple plannedExercises the user would pick 1 from during training session. In other words: alternative exercises.

    [SQLite.Ignore]
    public PlannedExercise PlannedExercise { get; set; }

    [Indexed]
    public DayOfWeek Day { get; set; }

    public float Priority { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public WorkoutPlanItem()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public WorkoutPlanItem(string guid, PlannedExercise plannedExercise, DayOfWeek day, float priority)
    {
        Guid = guid;
        PlannedExercise = plannedExercise;
        PlannedExerciseGuid = plannedExercise.Guid;
        Day = day;
        Priority = priority;
    }

    public void SetPlannedExercise(PlannedExercise plannedExercise)
    {
        if (plannedExercise != null &&
            string.Equals(PlannedExerciseGuid, plannedExercise?.Guid, StringComparison.InvariantCultureIgnoreCase))
            PlannedExercise = plannedExercise;
        else
            throw new Exception($"Provided {nameof(PlannedExercise)} guid '{plannedExercise?.Guid}' does not match expected '{PlannedExerciseGuid}'");
    }

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddWorkoutPlanItem(this);

    public Task Upsert(DatabaseHandler databaseHandler) => databaseHandler.UpsertWorkoutPlanItem(this);

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteWorkoutPlanItem(Guid);

    private ExerciseStat? LastStatsFirstEntry =>
        PlannedExercise.LastStats?.ExerciseStats?.FirstOrDefault();

    public int GetSets() => LastStatsFirstEntry?.Sets ?? PlannedExercise.Sets;

    public int GetReps() => LastStatsFirstEntry?.Reps ?? PlannedExercise.Reps;

    public float GetWeight() => LastStatsFirstEntry?.Weight ?? PlannedExercise.Weight;

    public bool GetDropSet() => LastStatsFirstEntry?.DropSet ?? PlannedExercise.DropSet;

    public bool GetToFailure() => LastStatsFirstEntry?.ToFailure ?? PlannedExercise.ToFailure;

    public string GetDropSet(Func<bool, string> boolStringFormatFunc) => boolStringFormatFunc(LastStatsFirstEntry?.DropSet ?? PlannedExercise.DropSet);

    public string GetToFailure(Func<bool, string> boolStringFormatFunc) => boolStringFormatFunc(LastStatsFirstEntry?.ToFailure ?? PlannedExercise.ToFailure);

    public IEnumerable<int> GetSetsArray()
    {
        if (LastStatsFirstEntry?.Sets == null)
            return new int[] { PlannedExercise.Sets };

        return PlannedExercise.LastStats.ExerciseStats.Select(x => x.Sets);
    }

    public IEnumerable<int> GetRepsArray()
    {
        if (LastStatsFirstEntry?.Reps == null)
            return new int[] { PlannedExercise.Reps };

        return PlannedExercise.LastStats.ExerciseStats.Select(x => x.Reps);
    }

    public IEnumerable<float> GetWeightArray()
    {
        if (LastStatsFirstEntry?.Weight == null)
            return new float[] { PlannedExercise.Weight };

        return PlannedExercise.LastStats.ExerciseStats.Select(x => x.Weight);
    }

    public IEnumerable<string> GetDropSetArray(Func<bool, string> boolStringFormatFunc)
    {
        if (LastStatsFirstEntry?.DropSet == null)
            return new string[] { boolStringFormatFunc(PlannedExercise.DropSet) };

        return PlannedExercise.LastStats.ExerciseStats.Select(x => boolStringFormatFunc(x.DropSet));
    }

    public IEnumerable<string> GetToFailureArray(Func<bool, string> boolStringFormatFunc)
    {
        if (LastStatsFirstEntry?.ToFailure == null)
            return new string[] { boolStringFormatFunc(PlannedExercise.ToFailure) };

        return PlannedExercise.LastStats.ExerciseStats.Select(x => boolStringFormatFunc(x.ToFailure));
    }
}