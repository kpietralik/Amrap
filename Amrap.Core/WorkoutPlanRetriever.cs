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
        var exerciseTypeModels = await _databaseHandler.GetExerciseTypes();
        // ToDo: convert to domain objects
        var exercisesTypes = new List<ExerciseType>();
        foreach (var exerciseType in exerciseTypeModels)
        {
            exercisesTypes.Add(ExerciseType.FromModel(exerciseType));
        }

        var plannedExerciseModels = await _databaseHandler.GetPlannedExercises();
        var plannedExercises = new List<PlannedExercise>();
        foreach (var plannedExercise in plannedExerciseModels)
        {
            var exercise = PlannedExercise.FromModel(
                plannedExercise, exercisesTypes.Single(x => x.Guid == plannedExercise.ExerciseTypeGuid));

            var lastStats = await _databaseHandler.GetLastStats(plannedExercise.Guid); // 1 to at most 1 relationship

            if (lastStats != null)
                exercise.LastStats = LastStats.FromModel(lastStats, exercise);

            plannedExercises.Add(exercise);
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