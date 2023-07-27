using Amrap.Core.Domain;
using Amrap.Core.Models;
using SQLite;

namespace Amrap.Infrastructure.Db;

public class DatabaseHandler
{
    private const string _dbName = "Amrap.db";
    private string _databasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _dbName);
    private SQLiteAsyncConnection _db;

    public bool HasInitialized => _db != null;

    public DatabaseHandler()
    {
    }

    public async Task CreateConnectionAndTables()
    {
        if (HasInitialized)
            return;

        _db = new SQLiteAsyncConnection(_databasePath);

        // ToDo: TEMP
#if DEBUG
        await _db.DropTableAsync<ExerciseType>();
        await _db.DropTableAsync<CompletedExercise>();
        await _db.DropTableAsync<PlannedExerciseModel>();
        await _db.DropTableAsync<WorkoutPlanItemModel>();
        await _db.DropTableAsync<LastStatsModel>();
#endif
        // ToDo: end

        await _db.CreateTableAsync<ExerciseType>();
        await _db.CreateTableAsync<CompletedExercise>();
        await _db.CreateTableAsync<PlannedExerciseModel>();
        await _db.CreateTableAsync<WorkoutPlanItemModel>();
        await _db.CreateTableAsync<LastStatsModel>();
    }

    // SEED
    public async Task SeedExerciseTypes(IList<ExerciseType> exerciseTypes)
    {
        var dbExerciesTypes = await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)}");

        foreach (var exerciseType in exerciseTypes)
        {
            if (dbExerciesTypes.Any(x => string.Equals(
                    x.Name,
                    exerciseType.Name,
                    StringComparison.InvariantCultureIgnoreCase)))
                continue;

            await AddExerciseType(exerciseType);
        }
    }

    // WRITE
    public Task AddExerciseType(ExerciseType exerciseType) => _db.InsertAsync(exerciseType);

    public Task UpdateExerciseType(ExerciseType exerciseType) => _db.UpdateAsync(exerciseType);

    public Task AddExercise(CompletedExercise exercise) => _db.InsertAsync(exercise);

    public Task DeleteCompletedExercise(int id) => _db.DeleteAsync<CompletedExercise>(id);

    public Task AddPlannedExercise(PlannedExerciseModel plannedExercise) => _db.InsertAsync(plannedExercise);

    public Task UpdatePlannedExercise(PlannedExerciseModel plannedExercise) => _db.UpdateAsync(plannedExercise);

    public Task AddWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem) => _db.InsertAsync(workoutPlanItem);

    public Task UpdateWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem) => _db.UpdateAsync(workoutPlanItem);

    public Task DeleteWorkoutPlanItem(string guid) => _db.DeleteAsync<WorkoutPlanItemModel>(guid);

    public Task SetLastStats(LastStatsModel lastStatsModel) => _db.InsertOrReplaceAsync(lastStatsModel);

    // READ
    public async Task<IList<ExerciseType>> GetExerciseTypes() =>
        await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)}");

    public async Task<ExerciseType> GetExerciseType(string guid)
    {
        var res = await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)} where Guid = ?", guid);

        return res.Single();
    }

    public async Task<IList<PlannedExerciseModel>> GetPlannedExercises()
    {
        var res = await _db.QueryAsync<PlannedExerciseModel>($"select * from {nameof(PlannedExerciseModel)}");

        return res;
    }

    public async Task<IList<WorkoutPlanItemModel>> GetWorkoutPlan() =>
        await _db.QueryAsync<WorkoutPlanItemModel>($"select * from {nameof(WorkoutPlanItemModel)}");

    public async Task<IList<CompletedExercise>> GetCompletedExercises(IList<ExerciseType> exerciseTypes)
    {
        var completedExercises = await _db.QueryAsync<CompletedExercise>($"select * from {nameof(CompletedExercise)}");

        foreach (var completedExercise in completedExercises)
        {
            completedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == completedExercise.ExerciseTypeGuid));
        }

        return completedExercises;
    }

    public async Task<LastStatsModel?> GetLastStats(string guid)
    {
        var res = await _db.QueryAsync<LastStatsModel>($"select * from {nameof(LastStatsModel)} where Guid = ?", guid);

        return res.SingleOrDefault();
    }
}