using EvernoteClone.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel.Helpers
{
    internal class DatabaseHelperContext : DbContext
    {

        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notebook> Notebooks { get; set; }

        public DatabaseHelperContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=pashamed\sqlexpress;Initial Catalog=EvernoteClone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

    public interface IAppEntity<T> where T : class, IEntity<string>
    {
        public Task<bool> Create<T>(T entity);
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        public Task<List<T>> GetAll<T>() where T : class;
        public Task<T> GetById<T>(string id) where T : class, IEntity<string>;
    }

    

    public class MsSqlDbProvider : IAppEntity<IEntity<string>>
    {
        private DatabaseHelperContext _repository;

        public MsSqlDbProvider()
        {
            _repository = new DatabaseHelperContext();
        }

        public async Task<bool> Create<TEntity>(TEntity entity)
        {
            await _repository.AddAsync(entity);
            var result = await _repository.SaveChangesAsync();
            return result > 0 ? true : false;
        }

        public async void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _repository.Remove<TEntity>(entity);
            await _repository.SaveChangesAsync();
        }

        public async void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _repository.Update<TEntity>(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await _repository.Set<TEntity>().ToListAsync<TEntity>();
        }

        public async Task<T> GetById<T>(string id) where T : class, IEntity<string>
        {
            return await _repository.Set<T>().SingleAsync(x => x.Id == id );
        }
    }

    public class FirebaseDbProvider : IAppEntity<IEntity<string>>
    {
        private static string dbPath = "https://notes-app-wpf-pavlo-default-rtdb.europe-west1.firebasedatabase.app/";

        public async Task<bool> Create<TEntity>(TEntity entity)
        {
            var jsonBody = JsonSerializer.Serialize<TEntity>(entity);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json") { };
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{dbPath}{entity.GetType().Name.ToLower()}.json", content);
                if (result.IsSuccessStatusCode)
                {
                    MsSqlDbProvider msDbProvider = new MsSqlDbProvider();
                    var localSaveSuccess = await msDbProvider.Create(entity);
                    if (localSaveSuccess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }                   
            }
        }

        public void Delete<TEntity>(TEntity app) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetById<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetById<T>(string id) where T : class, IEntity<string>
        {
            throw new NotImplementedException();
        }

        public void Update<TEntity>(TEntity app) where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
