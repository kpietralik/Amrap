using Amrap.Core.Domain;
using Amrap.Infrastructure.Db;

namespace Amrap.Core;

public class ExerciseTypeRetriever
{
    private readonly DatabaseHandler _databaseHandler;

    public ExerciseTypeRetriever(DatabaseHandler databaseHandler)
    {
        _databaseHandler = databaseHandler;
    }

    public async Task<IList<ExerciseType>> GetExerciseTypes()
    {
        var exerciseTypeModels = await _databaseHandler.GetExerciseTypes();

        var exercisesTypes = new List<ExerciseType>();
        foreach (var exerciseType in exerciseTypeModels)
        {
            exercisesTypes.Add(ExerciseType.FromModel(exerciseType));
        }

        return exercisesTypes;
    }
}
