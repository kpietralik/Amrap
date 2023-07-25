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
        await _db.DropTableAsync<ExerciseTypeModel>();
        await _db.DropTableAsync<CompletedExerciseModel>();
        await _db.DropTableAsync<PlannedExerciseModel>();
        await _db.DropTableAsync<WorkoutPlanItemModel>();
        await _db.DropTableAsync<LastStatsModel>();
        // ToDo: end

		await _db.CreateTableAsync<ExerciseTypeModel>();
		await _db.CreateTableAsync<CompletedExerciseModel>();
		await _db.CreateTableAsync<PlannedExerciseModel>();
		await _db.CreateTableAsync<WorkoutPlanItemModel>();
		await _db.CreateTableAsync<LastStatsModel>();


        //var items1 = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)}");
        //var items2 = await _db.QueryAsync<CompletedExerciseModel>($"select * from {nameof(CompletedExerciseModel)}");
        //var items3 = await _db.QueryAsync<PlannedExerciseModel>($"select * from {nameof(PlannedExerciseModel)}");
        //var items4 = await _db.QueryAsync<WorkoutPlanItemModel>($"select * from {nameof(WorkoutPlanItemModel)}");
        //var items5 = await _db.QueryAsync<LastStatsModel>($"select * from {nameof(LastStatsModel)}");
    }

    // SEED
    public async Task SeedExerciseTypes(IList<ExerciseTypeModel> exerciseTypes)
    {
		var dbExerciesTypes = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)}");

        foreach (var exerciseType in exerciseTypes)
		{
			if (dbExerciesTypes.Any(x => string.Equals(x.Name, exerciseType.Name, StringComparison.InvariantCultureIgnoreCase)))
				continue;
	
			await AddExerciseType(exerciseType);
        }
    }

    // WRITE
    public async Task AddExerciseType(ExerciseTypeModel exerciseType)
	{
		await _db.InsertAsync(exerciseType);
    }

    public async Task UpdateExerciseType(ExerciseTypeModel exerciseType)
    {
        await _db.UpdateAsync(exerciseType);
    }

    public async Task DeleteExerciseType(ExerciseTypeModel exerciseType)
    {
		// ToDo: decide how to handle deletion in existing Exercise db items
        await _db.DeleteAsync<ExerciseTypeModel>(exerciseType);
    }

    public async Task AddExercise(CompletedExerciseModel exercise)
	{
		await _db.InsertAsync(exercise);

        //var items = await _db.QueryAsync<Exercise>($"select * from {nameof(CompletedExercise)}");
    }

    public async Task DeleteExercise(CompletedExerciseModel exercise)
    {
        await _db.DeleteAsync<CompletedExerciseModel>(exercise);
    }

    public async Task AddPlannedExercise(PlannedExerciseModel plannedExercise)
    {
        await _db.InsertAsync(plannedExercise);
    }

    public async Task DeletePlannedExercise(PlannedExerciseModel plannedExercise)
    {
        await _db.DeleteAsync<PlannedExerciseModel>(plannedExercise);
    }

    public async Task AddWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem)
    {
        await _db.InsertAsync(workoutPlanItem);
    }

    public async Task DeleteWorkoutPlanItem(WorkoutPlanItemModel workoutPlanItem)
    {
        await _db.DeleteAsync<WorkoutPlanItemModel>(workoutPlanItem);
    }

    public async Task SetLastStats(LastStatsModel lastStatsModel)
    {
        await _db.InsertOrReplaceAsync(lastStatsModel);

        var res0 = await _db.QueryAsync<LastStatsModel>($"select * from {nameof(LastStatsModel)}");

    }

    public async Task DeleteLastStats(LastStatsModel lastStatsModel)
    {
        await _db.DeleteAsync<LastStatsModel>(lastStatsModel);
    }

    // READ
    public async Task<IList<ExerciseTypeModel>> GetExerciseTypes()
    {
        var res = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)}");

        return res;
    }

    public async Task<ExerciseTypeModel> GetExerciseType(int id)
    {
        var res = await _db.GetAsync<ExerciseTypeModel>(id);

        return res;
    }

    public async Task<ExerciseTypeModel> GetExerciseType(string guid)
    {
        var res = await _db.QueryAsync<ExerciseTypeModel>($"select * from {nameof(ExerciseTypeModel)} where Guid = ?", guid);

        //if (res.Count == 0)
        //    throw new Exception($"{nameof(ExerciseTypeModel)} with guid {guid} not found");

        //if (res.Count > 1)
        //    throw new Exception($"{nameof(ExerciseTypeModel)} with guid {guid} has too many items");

        return res.Single();
    }

    public async Task<IList<PlannedExerciseModel>> GetPlannedExercises()
    {
        var res = await _db.QueryAsync<PlannedExerciseModel>($"select * from {nameof(PlannedExerciseModel)}");

        return res;
    }

    public async Task<PlannedExerciseModel> GetPlannedExercise(int id)
    {
        var res = await _db.GetAsync<PlannedExerciseModel>(id);

        return res;
    }

    public async Task<PlannedExerciseModel> GetPlannedExercise(string guid)
    {
        var res = await _db.QueryAsync<PlannedExerciseModel>($"select * from {nameof(PlannedExerciseModel)} where Guid = ?", guid);

        //if (res.Count == 0)
        //    throw new Exception($"{nameof(PlannedExerciseModel)} with guid {guid} not found");

        //if (res.Count > 1)
        //    throw new Exception($"{nameof(PlannedExerciseModel)} with guid {guid} has too many items");

        return res.Single();
    }

    public async Task<IList<WorkoutPlanItemModel>> GetWorkoutPlan()
    {
        var res = await _db.QueryAsync<WorkoutPlanItemModel>($"select * from {nameof(WorkoutPlanItemModel)}");

        return res;
    }

    public async Task<WorkoutPlanItemModel> GetWorkoutPlan(int id)
    {
        var res = await _db.GetAsync<WorkoutPlanItemModel>(id);

        return res;
    }

    public async Task<WorkoutPlanItemModel> GetWorkoutPlanItem(string guid)
    {
        var res = await _db.QueryAsync<WorkoutPlanItemModel>($"select * from {nameof(WorkoutPlanItemModel)} where Guid = ?", guid);

        //if (res.Count == 0)
        //    throw new Exception($"{nameof(WorkoutPlanItemModel)} with guid {guid} not found");

        //if (res.Count > 1)
        //    throw new Exception($"{nameof(WorkoutPlanItemModel)} with guid {guid} has too many items");

        return res.Single();
    }

    public async Task<IList<CompletedExerciseModel>> GetCompletedExercises()
    {
        var res = await _db.QueryAsync<CompletedExerciseModel>($"select * from {nameof(CompletedExerciseModel)}");

        return res;
    }

    public async Task<CompletedExerciseModel> GetCompletedExercise(int id)
    {
        var res = await _db.GetAsync<CompletedExerciseModel>(id);

        return res;
    }

    public async Task<CompletedExerciseModel> GetCompletedExercise(string guid)
    {
        var res = await _db.QueryAsync<CompletedExerciseModel>($"select * from {nameof(CompletedExerciseModel)} where Guid = ?", guid);

        //if (res.Count == 0)
        //    throw new Exception($"{nameof(CompletedExerciseModel)} with guid {guid} not found");

        //if (res.Count > 1)
        //    throw new Exception($"{nameof(CompletedExerciseModel)} with guid {guid} has too many items");

        return res.Single();
    }

    public async Task<LastStatsModel?> GetLastStats(string guid)
    {
        var res = await _db.QueryAsync<LastStatsModel>($"select * from {nameof(LastStatsModel)} where Guid = ?", guid);

        //if (res.Count > 1)
        //    throw new Exception($"{nameof(LastStatsModel)} with guid {guid} has too many items");

        return res.SingleOrDefault();
    }
}
