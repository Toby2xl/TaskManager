using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using Taskman.Application.Interfaces;

namespace Taskman.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IConfiguration _configuration;

        public TaskRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> Add(core.Entities.Task entity)
        {
            entity.DateCreated = DateTime.Now;
            var sql = "INSERT INTO Tasks (Name, Description, Status, DueDate, DateCreated) Values (@Name, @Description, @Status, @DueDate, @DateCreated);";
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }

        public async Task<int> Delete(int id)
        {
            var sql = "DELETE FROM Tasks WHERE Id = @Id;";
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows;
            }
        }

        public async Task<core.Entities.Task> Get(int id)
        {
            var sql = "SELECT * FROM Tasks WHERE Id = @Id;";
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<core.Entities.Task>(sql, new { Id = id });
                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<core.Entities.Task>> GetAll()
        {
            var sql = "SELECT * FROM Tasks;";
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<core.Entities.Task>(sql);
                return result;
            }
        }

        public async Task<int> Update(core.Entities.Task entity)
        {
            entity.DateModified = DateTime.Now;
            var sql = "UPDATE Tasks SET Name = @Name, Description = @Description, Status = @Status, DueDate = @DueDate, DateModified = @DateModified WHERE Id = @Id;";
            using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var affectedRows = await connection.ExecuteAsync(sql, entity);
                return affectedRows;
            }
        }
    }
}