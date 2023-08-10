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

    private PlannedExercise _plannedExercise;
    public PlannedExercise PlannedExercise => _plannedExercise;

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

    public LastStats()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    { }

    public LastStats(PlannedExercise plannedExercise, int sets, int reps, float weight, bool dropSet, bool toFailure)
    {
        // At most 1 last stats for each planned exercise
        Guid = plannedExercise.Guid;
        _plannedExercise = plannedExercise;
        PlannedExerciseGuid = plannedExercise.Guid;
        Sets = sets;
        Reps = reps;
        Weight = weight;
        DropSet = dropSet;
        ToFailure = toFailure;
    }

    public void SetPlannedExercise(PlannedExercise plannedExercise)
    {
        if (plannedExercise != null &&
            string.Equals(PlannedExerciseGuid, plannedExercise?.Guid, StringComparison.InvariantCultureIgnoreCase))
            _plannedExercise = plannedExercise;
        else
            throw new Exception($"Provided {nameof(PlannedExercise)} guid '{plannedExercise?.Guid}' does not match expected '{PlannedExerciseGuid}'");
    }

    public Task Save(DatabaseHandler databaseHandler) => databaseHandler.SetLastStats(this);
}