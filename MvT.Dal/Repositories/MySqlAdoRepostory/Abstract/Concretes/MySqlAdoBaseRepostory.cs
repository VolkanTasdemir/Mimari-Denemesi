using Microsoft.Extensions.Options;
using MvT.Dal.Context;
using MvT.Dal.Entities;
using MvT.Entities.Interface;
using MySql.Data.MySqlClient;

namespace MvT.Dal.Repositories.MySqlRepostory.Abstract.Concretes
{
    public class MySqlAdoBaseRepostory<T> where T : class, IDbEntity
    {
        //private readonly eLogin _login;

        //private readonly string connectionString;

        //public MySqlAdoBaseRepostory(eLogin login)
        //{
        //    _login = login;
        //}

        //public void InsertWithTransaction(IEnumerable<TEntity> entities)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(_login.connStr))
        //    {
        //        connection.Open();
        //        using (MySqlTransaction transaction = connection.BeginTransaction())
        //        {
        //            try
        //            {
        //                foreach (var entity in entities)
        //                {
        //                    Insert(entity, connection, transaction);
        //                }
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                // Bir hata oluştuğunda rollback yap
        //                Console.WriteLine($"İşlem başarısız. Geri dönüyoruz. Hata: {ex.Message}");
        //                transaction.Rollback();
        //            }
        //        }
        //    }
        //}

        //private void Insert(TEntity entity, SqlConnection connection, SqlTransaction transaction)
        //{
        //    string tableName = typeof(TEntity).Name;
        //    string columnNames = string.Join(", ", typeof(TEntity).GetProperties().Select(p => p.Name));
        //    string parameterNames = string.Join(", ", typeof(TEntity).GetProperties().Select(p => $"@{p.Name}"));

        //    string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames})";

        //    using (SqlCommand command = new SqlCommand(insertQuery, connection, transaction))
        //    {
        //        foreach (var property in typeof(TEntity).GetProperties())
        //        {
        //            command.Parameters.AddWithValue($"@{property.Name}", property.GetValue(entity));
        //        }

        //        command.ExecuteNonQuery();
        //    }
        //}
    //}

    //MySqlDatabaseManager _db;

        //public MySqlAdoBaseRepostory(MySqlDatabaseManager db)
        //{
        //    //_db = db;
        //}

        public Task Insert(T rec)
        {
            throw new NotImplementedException();
        }

        public Task InsertRange(List<T> recList)
        {
            throw new NotImplementedException();
        }

        public Task Update(T rec)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRange(List<T> recList)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T Rec)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(List<T> recList)
        {
            throw new NotImplementedException();
        }

        public Task<T> Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetModifieds()
        {
            throw new NotImplementedException();
        }

        
    }
}
