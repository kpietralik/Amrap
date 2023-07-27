﻿using Amrap.Core.Domain;
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

    public async Task CreateConnectionAndTables(bool force = false)
    {
        if (HasInitialized && !force)
            return;

        _db = new SQLiteAsyncConnection(_databasePath);
        
        if (force)
        {
            // This will remove all of application's data.
            await _db.DropTableAsync<ExerciseType>();
            await _db.DropTableAsync<CompletedExercise>();
            await _db.DropTableAsync<PlannedExercise>();
            await _db.DropTableAsync<WorkoutPlanItem>();
            await _db.DropTableAsync<LastStats>();
        }

        await _db.CreateTableAsync<ExerciseType>();
        await _db.CreateTableAsync<CompletedExercise>();
        await _db.CreateTableAsync<PlannedExercise>();
        await _db.CreateTableAsync<WorkoutPlanItem>();
        await _db.CreateTableAsync<LastStats>();
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

    public Task AddPlannedExercise(PlannedExercise plannedExercise) => _db.InsertAsync(plannedExercise);

    public Task UpdatePlannedExercise(PlannedExercise plannedExercise) => _db.UpdateAsync(plannedExercise);

    public Task AddWorkoutPlanItem(WorkoutPlanItem workoutPlanItem) => _db.InsertAsync(workoutPlanItem);

    public Task UpdateWorkoutPlanItem(WorkoutPlanItem workoutPlanItem) => _db.UpdateAsync(workoutPlanItem);

    public Task DeleteWorkoutPlanItem(string guid) => _db.DeleteAsync<WorkoutPlanItem>(guid);

    public Task SetLastStats(LastStats lastStatsModel) => _db.InsertOrReplaceAsync(lastStatsModel);

    // READ
    public async Task<IList<ExerciseType>> GetExerciseTypes() =>
        await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)}");

    public async Task<ExerciseType> GetExerciseType(string guid)
    {
        var res = await _db.QueryAsync<ExerciseType>($"select * from {nameof(ExerciseType)} where Guid = ?", guid);

        return res.Single();
    }

    public async Task<IList<PlannedExercise>> GetPlannedExercises(IList<ExerciseType> exerciseTypes)
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

        var plannedExercise = await GetPlannedExercise(workoutPlanItem.PlannedExerciseGuid);

        workoutPlanItem.SetPlannedExercise(plannedExercise);

        return workoutPlanItem;
    }

    public async Task<IList<WorkoutPlanItem>> GetWorkoutPlan(IList<PlannedExercise> plannedExercises)
    {
        var workoutPlans = await _db.QueryAsync<WorkoutPlanItem>($"select * from {nameof(WorkoutPlanItem)}");

        foreach (var workoutPlan in workoutPlans)
        {
            workoutPlan.SetPlannedExercise(plannedExercises.Single(x => x.Guid == workoutPlan.PlannedExerciseGuid));
        }

        return workoutPlans;
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

    public async Task<LastStats?> GetLastStats(string guid)
    {
        var res = await _db.QueryAsync<LastStats>($"select * from {nameof(LastStats)} where Guid = ?", guid);

        return res.SingleOrDefault();
    }
}