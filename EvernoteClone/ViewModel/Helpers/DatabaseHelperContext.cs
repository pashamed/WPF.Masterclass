using EvernoteClone.Model;
using EvernoteClone.View;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        public DatabaseHelperContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(@"Data Source=pashamed\sqlexpress;Initial Catalog=EvernoteClone;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

    public interface IAppEntity<T> where T : class, IHasId<string>
    {
        public Task<bool> Create<T>(T entity);

        public Task<bool> Update<T>(T entity) where T : class, IHasId<string>;

        public Task<bool> Delete<T>(T entity) where T : class, IHasId<string>;

        public Task<List<T>> GetAll<T>() where T : class;

        public Task<T> GetById<T>(string id) where T : class, IHasId<string>;
    }

    public class MsSqlDbProvider : IAppEntity<IHasId<string>>
    {
        private readonly DatabaseHelperContext _repository;

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

        public async Task<bool> Delete<TEntity>(TEntity entity) where TEntity : class, IHasId<string>
        {
            _repository.Remove<TEntity>(entity);
            if (entity is Notebook)
            {
                var notes = _repository.Notes.Where<Note>(n => n.Notebook.Id == entity.Id);
                foreach (var note in notes)
                {
                    if(File.Exists(note.FileLocation))
                    {
                        File.Delete(note.FileLocation);
                    }
                }
            }else if(entity is Note)
            {
                if(File.Exists((entity as Note).FileLocation))
                {
                    File.Delete((entity as Note).FileLocation);
                }
            }
            if (await _repository.SaveChangesAsync() > 0)
                return true;
            else
                return false;
        }

        public async Task<bool> Update<TEntity>(TEntity entity) where TEntity : class, IHasId<string>
        {
            _repository.Update<TEntity>(entity);
            if (await _repository.SaveChangesAsync() > 0)
                return true;
            else
                return false;
        }

        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await _repository.Set<TEntity>().ToListAsync<TEntity>();
        }

        public async Task<T> GetById<T>(string id) where T : class, IHasId<string>
        {
            return await _repository.Set<T>().SingleAsync(x => x.Id == id);
        }
    }

    public class FirebaseDbProvider : IAppEntity<IHasId<string>>
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> Delete<TEntity>(TEntity entity) where TEntity : class, IHasId<string>
        {
            var jsonBody = JsonSerializer.Serialize<TEntity>(entity);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json") { };
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.DeleteAsync($"{dbPath}{entity.GetType().Name.ToLower()}/{entity.Id}.json");
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string jsonResult = "";
                try
                {
                    jsonResult = await httpClient.GetStringAsync($"{dbPath}{typeof(TEntity).Name.ToLower()}.json");
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
                var entities = JsonSerializer.Deserialize<Dictionary<string, TEntity>>(jsonResult);
                List<TEntity> list = new List<TEntity>();
                foreach (var e in entities)
                {
                    list.Add(e.Value);
                }
                return list;
            }
        }

        public async Task<T> GetById<T>(string id) where T : class, IHasId<string>
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update<TEntity>(TEntity entity) where TEntity : class, IHasId<string>
        {
            var jsonBody = JsonSerializer.Serialize<TEntity>(entity);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json") { };
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PatchAsync($"{dbPath}{entity.GetType().Name.ToLower()}/{entity.Id}.json", content);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}