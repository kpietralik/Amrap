using Amrap.Core.Models;

namespace Amrap.Core.Domain;

public class WorkoutPlanItem
{
    public int Id { get; set; }
    public string Guid { get; set; }

    public PlannedExercise PlannedExercise { get; set; }

    public DayOfTheWeek Day { get; set; }

    public string Link => $"/WorkoutPlanItem/{Id}";

    public static WorkoutPlanItem FromModel(WorkoutPlanItemModel model, PlannedExercise plannedExercise)
    {
        if (model.PlannedExerciseGuid != plannedExercise.Guid.ToString())
            throw new Exception($"Data id missmatch: {nameof(WorkoutPlanItemModel)}, model={model.PlannedExerciseGuid}, data={plannedExercise.Guid}");

        return new(model.Id, model.Guid, plannedExercise, model.Day);
    }

    private WorkoutPlanItem(int id, string guid, PlannedExercise plannedExercise, DayOfTheWeek day)
    {
        Id = id;
        Guid = guid;
        PlannedExercise = plannedExercise;
        Day = day;
    }
}
