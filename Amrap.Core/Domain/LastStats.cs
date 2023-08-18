using Amrap.Core.Infrastructure;
using SQLite;

namespace Amrap.Core.Domain;

public class LastStats
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string PlannedExerciseGuid { get; set; }

    // ToDo: encapsulate
    [SQLite.Ignore]
    public LinkedList<ExerciseStat> ExerciseStats { get; set; } = new LinkedList<ExerciseStat>();

    /// <remarks>
    /// SQLite only
    /// </remarks>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public LastStats()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public LastStats(PlannedExercise plannedExercise)
    {
        // At most 1 last stats for each planned exercise
        Guid = plannedExercise.Guid;
        PlannedExerciseGuid = plannedExercise.Guid;
    }

    private Stack<ExerciseStat> _statsToRemove = new Stack<ExerciseStat>();
    private Stack<ExerciseStat> _statsToAdd = new Stack<ExerciseStat>();

    public void AddStat(int sets, int reps, float weight, bool dropSet, bool toFailure)
    {
        var stat = new ExerciseStat(System.Guid.NewGuid().ToString(), this, sets, reps, weight, dropSet, toFailure);

        _statsToAdd.Push(stat);
        ExerciseStats.AddFirst(stat);

        if (ExerciseStats.Last != null && ExerciseStats.Count > 5 ) // Storing max 5 last stats
        {
            _statsToRemove.Push(ExerciseStats.Last.Value);
            ExerciseStats.RemoveLast();
        }
    }

    public static Task<LastStats?> GetLastStatsFor(DatabaseHandler databaseHandler, PlannedExercise plannedExercise)
    {
        return databaseHandler.GetLastStats(plannedExercise.Guid);
    }

    public async Task Save(DatabaseHandler databaseHandler)
    {
        while (_statsToRemove.TryPop(out var stat))
            await databaseHandler.DeleteExerciseStats(stat.Guid);

        while (_statsToAdd.TryPop(out var stat))
            await databaseHandler.UpsertExerciseStats(stat);

        await databaseHandler.SetLastStats(this);
    }

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteLastStats(this);


}

public class ExerciseStat
{
    [PrimaryKey]
    public string Guid { get; set; }

    /// <remarks>
    /// SQLite only
    /// </remarks>
    [Indexed]
    public string LastStatsGuid { get; set; }

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

    public ExerciseStat()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    // Should only be called by LastStats.AddStat(..) method. Encapsulate.
    public ExerciseStat(string guid, LastStats lastStats, int sets, int reps, float weight, bool dropSet, bool toFailure)
    {
        Guid = guid;
        LastStatsGuid = lastStats.Guid;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }
}