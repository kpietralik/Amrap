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

    public Task Add(DatabaseHandler databaseHandler) => databaseHandler.AddWorkoutPlanItem(
            new WorkoutPlanItemModel(
                Guid,
                Day,
                PlannedExercise.Guid));

    public Task Update(DatabaseHandler databaseHandler) => databaseHandler.UpdateWorkoutPlanItem(
            new WorkoutPlanItemModel(
                Guid,
                Day,
                PlannedExercise.Guid));

    public Task Delete(DatabaseHandler databaseHandler) => databaseHandler.DeleteWorkoutPlanItem(Guid);

    public int GetSets() => PlannedExercise.LastStats?.Sets ?? PlannedExercise.Sets;
    public int GetReps() => PlannedExercise.LastStats?.Reps ?? PlannedExercise.Reps;
    public float GetWeight() => PlannedExercise.LastStats?.Weight ?? PlannedExercise.Weight;
    public bool GetDropSet() => PlannedExercise.LastStats?.DropSet ?? PlannedExercise.DropSet;
    public bool GetToFailure() => PlannedExercise.LastStats?.ToFailure ?? PlannedExercise.ToFailure;
}
