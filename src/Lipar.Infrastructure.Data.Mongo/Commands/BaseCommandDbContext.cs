using Lipar.Core.Contract.Common;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Events;
using Lipar.Core.Domain.Entities;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Data.Mongo.NewFolder;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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
    public IServiceProvider ServiceProvider { get; }

    private readonly List<Func<Task>> _commands = new();
    private readonly List<DomainEvent> _events = new();
    private static bool registeredAllSerializer = false;

    private BaseCommandDbContext() { }

    protected BaseCommandDbContext(IServiceProvider serviceProvider)
    {
        MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;
        var liparOptions = serviceProvider.GetService<LiparOptions>();
        MongoClient = new MongoClient(liparOptions.MongoDb.Connection);
        Database = MongoClient.GetDatabase(liparOptions.MongoDb.DatabaseName);
        ServiceProvider = serviceProvider;


        if (!registeredAllSerializer)
        {
            BsonSerializer.RegisterSerializer(new EntityIdSerializer(BsonSerializer.SerializerRegistry.GetSerializer<Guid?>()));
            registeredAllSerializer = true;
        }

        //Database.Settings.GuidRepresentation = GuidRepresentation.Standard;
    }

    public int SaveChanges()
    {
        //using (Session = MongoClient.StartSession())
        //    Session.StartTransaction();

        PublishEvents(DomainEventType.BeforeCommit).GetAwaiter();

        var commandTasks = _commands.Select(c => c());

        Task.WhenAll(commandTasks);

        AddEntityChangesInterceptors().GetAwaiter();
        PublishEvents(DomainEventType.AfterCommit).GetAwaiter();
        //Session.CommitTransaction();

        return _commands.Count;
    }


    #region Commit Process

    public async Task<int> SaveChangesAsync()
    {
        //using (Session = await MongoClient.StartSessionAsync())
        //Session.StartTransaction();

        var commandTasks = _commands.Select(c => c());

        await PublishEvents(DomainEventType.BeforeCommit);

        await Task.WhenAll(commandTasks);

        await AddEntityChangesInterceptors();
        await PublishEvents(DomainEventType.AfterCommit);
        //await Session.CommitTransactionAsync();

        return _commands.Count;
    }

    private async Task AddEntityChangesInterceptors()
    {
        /// must be develop
        var entityChangesInterceptors = new List<EntityChangesInterception>();
        var repository = ServiceProvider.GetService<IEntityChangesInterceptorRepository>();

        await repository.AddEntityChanges(entityChangesInterceptors);
    }

    private async Task PublishEvents(DomainEventType domainEventType)
    {
        var eventPublisher = ServiceProvider.GetService<IDomainEventDispatcher>();

        foreach (var @event in _events.Where(m => m.DomainEventType == domainEventType))
            await eventPublisher.Raise(@event);
    }

    #endregion




    public void AddCommand(Func<Task> func) => _commands.Add(func);
    public void AddEvents(IEnumerable<DomainEvent> events) => _events.AddRange(events);

    public IMongoCollection<T> GetCollection<T>(string name) => Database.GetCollection<T>(name);

    public void Dispose()
    {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }

    public object GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }
}

