﻿using Amrap.Core.Models;
using Amrap.Infrastructure.Db;

namespace Amrap.Core;

public class CompletedExerciseSaver
{
    private readonly DatabaseHandler _databaseHandler;

    public CompletedExerciseSaver(DatabaseHandler databaseHandler)
    {
        _databaseHandler = databaseHandler;
    }

    //public async Task SaveCompletedExercise(CompletedExercise completedExercise)
    //{
    //    await _databaseHandler.AddExercise(completedExercise.ToModel());
    //}    
    
    public async Task SaveCompletedExercise(CompletedExerciseModel completedExerciseModel)
    {
        await _databaseHandler.AddExercise(completedExerciseModel);
    }
}
