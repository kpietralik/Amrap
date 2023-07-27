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
        await _db.DropTableAsync<ExerciseTypeModel>();
        await _db.DropTableAsync<CompletedExerciseModel>();
        await _db.DropTableAsync<PlannedExerciseModel>();
        await _db.DropTableAsync<WorkoutPlanItemModel>();
        await _db.DropTableAsync<LastStatsModel>();
#endif
        // ToDo: end

        await _db.CreateTableAsync<ExerciseTypeModel>();
        await _db.CreateTableAsync<CompletedExerciseModel>();
        await _db.CreateTableAsync<PlannedExerciseModel>();
        await _db.CreateTableAsync<WorkoutPlanItemModel>();
        await _db.CreateTableAsync<LastStatsModel>();
    }

    // SEED
    public async Task SeedExerciseTypes(IList<ExerciseTypeModel> exerciseTypes)
    {
        var dbExerciesTypes = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)}");

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
    public Task AddExerciseType(ExerciseTypeModel exerciseType) => _db.InsertAsync(exerciseType);

    public Task UpdateExerciseType(ExerciseTypeModel exerciseType) => _db.UpdateAsync(exerciseType);

    public Task AddExercise(CompletedExerciseModel exercise) => _db.InsertAsync(exercise);

    public Task DeleteCompletedExercise(int id) => _db.DeleteAsync<CompletedExerciseModel>(id);

    public Task AddPlannedExercise(PlannedExerciseModel plannedExercise) => _db.InsertAsync(plannedExercise);

    public Task UpdatePlannedExercise(PlannedExerciseModel plannedExercise) => _db.UpdateAsync(plannedExercise);

    public Task AddWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem) => _db.InsertAsync(workoutPlanItem);

    public Task UpdateWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem) => _db.UpdateAsync(workoutPlanItem);

    public Task DeleteWorkoutPlanItem(string guid) => _db.DeleteAsync<WorkoutPlanItemModel>(guid);

    public Task SetLastStats(LastStatsModel lastStatsModel) => _db.InsertOrReplaceAsync(lastStatsModel);

    // READ
    public async Task<IList<ExerciseTypeModel>> GetExerciseTypes() =>
        await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)}");

    public async Task<ExerciseTypeModel> GetExerciseType(string guid)
    {
        var res = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)} where Guid = ?", guid);

        return res.Single();
    }

    public async Task<IList<PlannedExerciseModel>> GetPlannedExercises()
    {
        var res = await _db.QueryAsync<PlannedExerciseModel>($"select * from {nameof(PlannedExerciseModel)}");

        return res;
    }

    public async Task<IList<WorkoutPlanItemModel>> GetWorkoutPlan() =>
        await _db.QueryAsync<WorkoutPlanItemModel>($"select * from {nameof(WorkoutPlanItemModel)}");

    public async Task<IList<CompletedExerciseModel>> GetCompletedExercises() =>
        await _db.QueryAsync<CompletedExerciseModel>($"select * from {nameof(CompletedExerciseModel)}");

    public async Task<LastStatsModel?> GetLastStats(string guid)
    {
        var res = await _db.QueryAsync<LastStatsModel>($"select * from {nameof(LastStatsModel)} where Guid = ?", guid);

        return res.SingleOrDefault();
    }
}