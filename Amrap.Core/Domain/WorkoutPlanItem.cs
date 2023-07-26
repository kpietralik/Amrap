using Amrap.Core.Models;
using Amrap.Infrastructure.Db;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    public string Guid { get; set; }

    public PlannedExercise PlannedExercise { get; set; }

    public DayOfWeek Day { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    public static WorkoutPlanItem FromModel(WorkoutPlanItemModel model, PlannedExercise plannedExercise)
    {
        if (model.PlannedExerciseGuid != plannedExercise.Guid.ToString())
            throw new Exception($"Data id missmatch: {nameof(WorkoutPlanItemModel)}, model={model.PlannedExerciseGuid}, data={plannedExercise.Guid}");

        return new(model.Guid, plannedExercise, model.Day);
    }

    public WorkoutPlanItem(string guid, PlannedExercise plannedExercise, DayOfWeek day)
    {
        Guid = guid;
        PlannedExercise = plannedExercise;
        Day = day;
    }

    public async Task Add(DatabaseHandler databaseHandler)
    {
        await databaseHandler.AddWorkoutPlanItem(
            new WorkoutPlanItemModel(
                Guid,
                Day,
                PlannedExercise.Guid));
    }

    public async Task Update(DatabaseHandler databaseHandler)
    {
        await databaseHandler.UpdateWorkoutPlanItem(
            new WorkoutPlanItemModel(
                Guid,
                Day,
                PlannedExercise.Guid));
    }

    public int GetSets() => PlannedExercise.LastStats?.Sets ?? PlannedExercise.Sets;
    public int GetReps() => PlannedExercise.LastStats?.Reps ?? PlannedExercise.Reps;
    public float GetWeight() => PlannedExercise.LastStats?.Weight ?? PlannedExercise.Weight;
    public bool GetDropSet() => PlannedExercise.LastStats?.DropSet ?? PlannedExercise.DropSet;
    public bool GetToFailure() => PlannedExercise.LastStats?.ToFailure ?? PlannedExercise.ToFailure;
}
