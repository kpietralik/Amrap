using Amrap.Core.Domain;
using Amrap.Core.Infrastructure;

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

        var workoutPlans = await _databaseHandler.GetWorkoutPlan(plannedExercises);

        return workoutPlans;
    }
}