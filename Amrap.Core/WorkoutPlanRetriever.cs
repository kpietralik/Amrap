using Amrap.Core.Domain;
using Amrap.Infrastructure.Db;

namespace Amrap.Core;

public class WorkoutPlanRetriever
{
    private readonly DatabaseHandler _databaseHandler;

    public WorkoutPlanRetriever(DatabaseHandler databaseHandler)
    {
        _databaseHandler = databaseHandler;
    }

    public async Task<IList<WorkoutPlanItem>> GetWorkoutPlan()
    {
        var exerciseTypes = await _databaseHandler.GetExerciseTypes();

        var plannedExercises = await _databaseHandler.GetPlannedExercises(exerciseTypes);
        foreach (var plannedExercise in plannedExercises)
        {
            var lastStats = await _databaseHandler.GetLastStats(plannedExercise.Guid); // 1 to at most 1 relationship

            if (lastStats != null)
                plannedExercise.SetLastStats(LastStats.FromModel(lastStats, plannedExercise));
        }

        var workoutPlanModels = await _databaseHandler.GetWorkoutPlan();
        var workoutPlans = new List<WorkoutPlanItem>();
        foreach (var workoutPlan in workoutPlanModels)
        {
            workoutPlans.Add(
                WorkoutPlanItem.FromModel(workoutPlan, plannedExercises.Single(x => x.Guid == workoutPlan.PlannedExerciseGuid)));
        }

        return workoutPlans;
    }
}