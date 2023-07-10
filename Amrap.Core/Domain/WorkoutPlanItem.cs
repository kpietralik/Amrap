using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    public string Guid { get; set; }

    public PlannedExercise PlannedExercise { get; set; }

    public DayOfTheWeek Day { get; set; }

    public string Link => $"/WorkoutPlanItem/{Guid}";

    public static WorkoutPlanItem FromModel(WorkoutPlanItemModel model, PlannedExercise plannedExercise)
    {
        if (model.PlannedExerciseGuid != plannedExercise.Guid.ToString())
            throw new Exception($"Data id missmatch: {nameof(WorkoutPlanItemModel)}, model={model.PlannedExerciseGuid}, data={plannedExercise.Guid}");

        return new(model.Guid, plannedExercise, model.Day);
    }

    private WorkoutPlanItem(string guid, PlannedExercise plannedExercise, DayOfTheWeek day)
    {
        Guid = guid;
        PlannedExercise = plannedExercise;
        Day = day;
    }

    public int GetSets() => PlannedExercise.LastStats?.Sets ?? PlannedExercise.Sets;
    public int GetReps() => PlannedExercise.LastStats?.Reps ?? PlannedExercise.Reps;
    public float GetWeight() => PlannedExercise.LastStats?.Weight ?? PlannedExercise.Weight;
    public bool GetDropSet() => PlannedExercise.LastStats?.DropSet ?? PlannedExercise.DropSet;
}
