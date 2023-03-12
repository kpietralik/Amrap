using Amrap.Core;
using SQLite;

namespace Amrap.Infrastructure.Db;

public class DatabaseHandler
{
	private const string _dbName = "Amrap.db";
	private string _databasePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _dbName);
	private SQLiteAsyncConnection _db;

	public bool HasConnection => _db != null;

	public DatabaseHandler()
	{
	}

	public async Task CreateConnectionAndTables()
	{
		_db = new SQLiteAsyncConnection(_databasePath);
		await _db.CreateTableAsync<Excercise>();
		//		await _db.CreateTableAsync<ExcerciseKind>();
	}

	public async Task AddExcercise(Excercise excercise)
	{

		await _db.InsertAsync(excercise);
	}
}
