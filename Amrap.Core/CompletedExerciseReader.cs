using Amrap.Core.Domain;
using Amrap.Core.Infrastructure;

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
        var exerciseTypes = await _databaseHandler.GetExerciseTypes();
        var completedExercises = await _databaseHandler.GetCompletedExercises(exerciseTypes);

        return completedExercises.OrderByDescending(x => x.Time);
    }
}