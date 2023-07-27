using Amrap.Core.Domain;
using Amrap.Infrastructure.Db;

namespace Amrap.Core;

public class CompletedExerciseReader
{
    private readonly DatabaseHandler _databaseHandler;

    public CompletedExerciseReader(DatabaseHandler databaseHandler)
    {
        _databaseHandler = databaseHandler;
    }

    public async Task<IOrderedEnumerable<CompletedExercise>> ReadCompletedExercies()
    {
        // First get ExerciseType models

        var exerciseTypeModels = await _databaseHandler.GetExerciseTypes();

        var exercisesTypes = new List<ExerciseType>();
        foreach (var exerciseType in exerciseTypeModels)
        {
            exercisesTypes.Add(ExerciseType.FromModel(exerciseType));
        }

        var completedExercises = await _databaseHandler.GetCompletedExercises(exercisesTypes);

        return completedExercises.OrderByDescending(x => x.Time);
    }
}