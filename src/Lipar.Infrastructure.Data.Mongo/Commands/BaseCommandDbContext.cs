using Lipar.Infrastructure.Tools.Utilities.Configurations;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Commands;

public abstract class BaseCommandDbContext
{
private IMongoDatabase Database { get; }
private IClientSessionHandle Session { get; set; }
private MongoClient MongoClient { get; set; }
    private readonly List<Func<Task>> _commands = new();
    
private BaseCommandDbContext() { }
    
    protected BaseCommandDbContext(LiparOptions liparOptions)
    {
        MongoClient = new MongoClient(liparOptions.MongoDb.Connection);
        Database = MongoClient.GetDatabase(liparOptions.MongoDb.DatabaseName);
    }
    
    public int SaveChanges()
    {
        using (Session = MongoClient.StartSession())
        {
            Session.StartTransaction();
            
            var commandTasks = _commands.Select(c => c());
            
            Task.WhenAll(commandTasks);
            
            Session.CommitTransaction();
        }
        
        return _commands.Count;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        using (Session = await MongoClient.StartSessionAsync())
        {
            Session.StartTransaction();
            
            var commandTasks = _commands.Select(c => c());
            
            await Task.WhenAll(commandTasks);
            
            await Session.CommitTransactionAsync();
        }
        
        return _commands.Count;
    }
    
    public void AddCommand(Func<Task> func) => _commands.Add(func);
    
    public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);
    
    public void Dispose()
    {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }
}

