using Amrap.Core.Domain;
using SQLite;

namespace Amrap.Core.Infrastructure;

public class DatabaseHandler
{
    private const string _dbName = "Amrap.db";
    private string _databasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _dbName);
    private SQLiteAsyncConnection _db;

    public bool HasInitialized => _db != null;

    public DatabaseHandler()
    {
    }

    // TODO: trim unused methods

    // Migration #1 -> multiple planned exercises for each workoutplanitem
    public async Task Migration1()
    {
        InitializeDb();
        await _db.CreateTableAsync<PlannedExercise>();

        var exerciseTypes = await GetExerciseTypes();
        var workoutPlanItems = await GetWorkoutPlan();
        var plannedExercises = await _db.QueryAsync<PlannedExercise>($"select * from {nameof(PlannedExercise)}");

        foreach (var plannedExercise in plannedExercises)
        {
            var workoutPlanItem = workoutPlanItems.Single(x => x.PlannedExerciseGuid == plannedExercise.Guid);

            plannedExercise.WorkoutPlanItemGuid = workoutPlanItem.Guid;

            await UpsertPlannedExercise(plannedExercise);
        }
    }

    private void InitializeDb()
    {
        if (!HasInitialized)
        {
            _db = new SQLiteAsyncConnection(_databasePath);
        }
    }

    public async Task CreateConnectionAndTables(bool force = false, bool keepAchievements = true)
    {
        if (HasInitialized && !force)
            return;

        InitializeDb();

        if (force)
        {
            // This will remove all of application's data.
            await _db.DropTableAsync<ExerciseType>();
            await _db.DropTableAsync<PlannedExercise>();
            await _db.DropTableAsync<WorkoutPlanItem>();

            if (!keepAchievements)
            {
                await _db.DropTableAsync<CompletedExercise>();
                await _db.DropTableAsync<LastStats>();
                await _db.DropTableAsync<ExerciseStat>();
            }
        }

        await _db.CreateTableAsync<ExerciseType>();
        await _db.CreateTableAsync<PlannedExercise>();
        await _db.CreateTableAsync<WorkoutPlanItem>();
        await _db.CreateTableAsync<CompletedExercise>();
        await _db.CreateTableAsync<LastStats>();
        await _db.CreateTableAsync<ExerciseStat>();
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
    
    public Task UpsertExerciseType(ExerciseType exerciseType) => _db.InsertOrReplaceAsync(exerciseType);

    public Task AddExercise(CompletedExercise exercise) => _db.InsertAsync(exercise);
    
    public Task UpsertExercise(CompletedExercise exercise) => _db.InsertOrReplaceAsync(exercise);

    public Task DeleteCompletedExercise(int id) => _db.DeleteAsync<CompletedExercise>(id);

    public Task AddPlannedExercise(PlannedExercise plannedExercise) => _db.InsertAsync(plannedExercise);

    public Task UpsertPlannedExercise(PlannedExercise plannedExercise) => _db.InsertOrReplaceAsync(plannedExercise);

    public async Task DeletePlannedExercise(PlannedExercise plannedExercise)
    {
        await DeleteLastStats(plannedExercise.LastStats);

        await _db.DeleteAsync<PlannedExercise>(plannedExercise.Guid);
    }

    public Task AddWorkoutPlanItem(WorkoutPlanItem workoutPlanItem) => _db.InsertAsync(workoutPlanItem);

    public Task UpsertWorkoutPlanItem(WorkoutPlanItem workoutPlanItem) => _db.InsertOrReplaceAsync(workoutPlanItem);

    public async Task DeleteWorkoutPlanItem(WorkoutPlanItem workoutPlanItem)
    {
        foreach (var ex in workoutPlanItem.PlannedExercises)
            await DeletePlannedExercise(ex);
     
        await _db.DeleteAsync<WorkoutPlanItem>(workoutPlanItem.Guid);
    }

    public async Task SetLastStats(LastStats lastStats)
    {
        // Save all stats first
        foreach (var item in lastStats.ExerciseStats)
        {
            await _db.InsertOrReplaceAsync(item);
        }

        await _db.InsertOrReplaceAsync(lastStats);
    }

    public async Task DeleteLastStats(LastStats lastStats)
    {
        if (lastStats == default)
            return;

        // Delete all stats first
        foreach (var item in lastStats.ExerciseStats)
        {
            await _db.DeleteAsync(item);
        }

        await _db.DeleteAsync(lastStats);
    }

    public Task DeleteExerciseStats(string guid) => _db.DeleteAsync<ExerciseStat>(guid);

    public Task UpsertExerciseStats(ExerciseStat exerciseStat) => _db.InsertOrReplaceAsync(exerciseStat);


    // READ
    public async Task<IList<ExerciseType>> GetExerciseTypes() =>
        await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)}");

    public async Task<ExerciseType> GetExerciseType(string guid)
    {
        var res = await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)} where Guid = ?", guid);

        return res.Single();
    }

    public async Task<IList<PlannedExercise>> GetAllPlannedExercises(IList<ExerciseType> exerciseTypes, IList<WorkoutPlanItem> workoutPlanItems)
    {
        var plannedExercises = await _db.QueryAsync<PlannedExercise>($"select * from {nameof(PlannedExercise)}");

        foreach (var plannedExercise in plannedExercises)
        {
            plannedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == plannedExercise.ExerciseTypeGuid));

            var lastStats = await GetLastStats(plannedExercise.Guid); // 1 to at most 1 relationship

            if (lastStats != null)
                plannedExercise.SetLastStats(lastStats);
        }

        return plannedExercises;
    }

    public async Task<IList<PlannedExercise>> GetPlannedExercisesForWorkoutPlanItem(WorkoutPlanItem workoutPlanItem, IList<ExerciseType> exerciseTypes)
    {
        var plannedExercisesForWorkoutPlanItem = await _db.QueryAsync<PlannedExercise>($"select * from {nameof(PlannedExercise)} where WorkoutPlanItemGuid = ?", workoutPlanItem.Guid);

        foreach (var plannedExercise in plannedExercisesForWorkoutPlanItem)
        {
            plannedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == plannedExercise.ExerciseTypeGuid));

            var lastStats = await GetLastStats(plannedExercise.Guid); // 1 to at most 1 relationship

            if (lastStats != null)
                plannedExercise.SetLastStats(lastStats);
        }

        return plannedExercisesForWorkoutPlanItem;
    }

    public async Task<PlannedExercise> GetPlannedExercise(string guid)
    {
        var res = await _db.QueryAsync<PlannedExercise>($"select * from {nameof(PlannedExercise)} where Guid = ?", guid);

        var plannedExercise = res.Single();

        var exerciseType = await GetExerciseType(plannedExercise.ExerciseTypeGuid);
        plannedExercise.SetExerciseType(exerciseType);

        var lastStats = await GetLastStats(plannedExercise.Guid);
        if (lastStats != default)
            plannedExercise.SetLastStats(lastStats);

        return plannedExercise;
    }

    public async Task<WorkoutPlanItem> GetWorkoutPlanItem(string guid)
    {
        var res = await _db.QueryAsync<WorkoutPlanItem>($"select * from {nameof(WorkoutPlanItem)} where Guid = ?", guid);

        var workoutPlanItem = res.Single();

        var exerciseTypes = await GetExerciseTypes();
        var plannedExercises = await GetPlannedExercisesForWorkoutPlanItem(workoutPlanItem, exerciseTypes);

        workoutPlanItem.SetPlannedExercises(plannedExercises);

        return workoutPlanItem;
    }

    public async Task<IList<WorkoutPlanItem>> GetWorkoutPlan()
    {
        var workoutPlan = await _db.QueryAsync<WorkoutPlanItem>($"select * from {nameof(WorkoutPlanItem)}");

        var exerciseTypes = await GetExerciseTypes();

        foreach(var workoutPlanItem in workoutPlan)
        {
            var plannedExercises = await GetPlannedExercisesForWorkoutPlanItem(workoutPlanItem, exerciseTypes);
            workoutPlanItem.SetPlannedExercises(plannedExercises);
        }


        return workoutPlan;
    }

    public async Task<IList<CompletedExercise>> GetCompletedExercises(IList<ExerciseType> exerciseTypes)
    {
        var completedExercises = await _db.QueryAsync<CompletedExercise>($"select * from {nameof(CompletedExercise)}");

        foreach (var completedExercise in completedExercises)
        {
            completedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == completedExercise.ExerciseTypeGuid));
        }

        return completedExercises;
    }

    public async Task<IEnumerable<CompletedExercise>> GetExercisesCompletedToday(DateTime today)
    {
        var ticksStartofToday = today.Date.Ticks;

        var completedExercises = await _db.QueryAsync<CompletedExercise>(
            $"select * from {nameof(CompletedExercise)} where " +
            $"Time > ?", ticksStartofToday);

        var exerciseTypes = await GetExerciseTypes();

        foreach (var completedExercise in completedExercises)
        {
            completedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == completedExercise.ExerciseTypeGuid));
        }
        
        return completedExercises;
    }

    public async Task<IEnumerable<CompletedExercise>> GetExercisesCompletedWithin(DateTime today)
    {
        var ticksStartofToday = today.Date.Ticks;

        var completedExercises = await _db.QueryAsync<CompletedExercise>(
            $"select * from {nameof(CompletedExercise)} where " +
            $"Time > ?", ticksStartofToday);

        var exerciseTypes = await GetExerciseTypes();

        foreach (var completedExercise in completedExercises)
        {
            completedExercise.SetExerciseType(exerciseTypes.Single(x => x.Guid == completedExercise.ExerciseTypeGuid));
        }

        return completedExercises;
    }

    public async Task<LastStats?> GetLastStats(string guid)
    {
        var res = await _db.QueryAsync<LastStats>($"select * from {nameof(LastStats)} where Guid = ?", guid);

        var lastStats = res.SingleOrDefault();

        if (lastStats == null)
            return null;

        // Get all stats
        var statsResult = await GetAllExerciseStatsForLastStats(lastStats.Guid);

        lastStats.ExerciseStats = statsResult;

        return lastStats;
    }

    public async Task<LinkedList<ExerciseStat>> GetAllExerciseStatsForLastStats(string lastStatsGuid)
    {
        var res = await _db.QueryAsync<ExerciseStat>($"select * from {nameof(ExerciseStat)} where LastStatsGuid = ?", lastStatsGuid);

        return new LinkedList<ExerciseStat>(res); ;
    }
}