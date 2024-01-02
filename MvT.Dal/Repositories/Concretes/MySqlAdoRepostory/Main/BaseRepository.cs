using MvT.Dal.DatabaseManager.MySqlDataBaseManger;
using MvT.Dal.Entities;
using MvT.Dal.Repositories.Abstract.Main;
using MvT.Entities.Interface;
using MySql.Data.MySqlClient;
using System.Data;

namespace MvT.Dal.Repositories.Concretes.MySqlAdoRepostory.Main
{
    public class BaseRepository<T> : IRepository<T> where T : class, IDbEntity
    {
        private readonly Login _Login;
        protected string _selectColums;
        protected string _joinTable;
        private readonly MySqlDatabaseManager<T> _MySqlDatabaseManager = new();

        IDbConnection? _dbConnection = null;
        IDbTransaction? _dbTransaction = null;

        public BaseRepository(Login login, string selectColums, string joinTable)
        {
            _Login = login;
            _selectColums = selectColums;
            _joinTable = joinTable;
        }

        public Task<T> Insert(T rec)
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("Transaction cannot be null.");
                var returnRec = _MySqlDatabaseManager.Insert(rec, _dbTransaction);
                if (transactionCheck) CommitTransaction();
                return returnRec;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public Task<List<T>> InsertRange(List<T> recList)
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("Transaction cannot be null.");
                Task<List<T>> returnRecList = _MySqlDatabaseManager.BulkInsert(recList, _dbTransaction);

                if (transactionCheck) CommitTransaction();
                return returnRecList;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public Task<T> Update(T rec)
        {
            bool transactionCheck = false;
            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }

            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("Transaction cannot be null.");
                var returnRec = _MySqlDatabaseManager.Update(rec, _dbTransaction);
                if (transactionCheck) CommitTransaction();
                return returnRec;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public Task<List<T>> UpdateRange(List<T> recList)
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("Transaction cannot be null.");
                Task<List<T>> returnRecList = _MySqlDatabaseManager.BulkUpdate(recList, _dbTransaction);

                if (transactionCheck) CommitTransaction();
                return returnRecList;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> Delete(T rec)
        {
            bool transactionCheck = false;
            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }

            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("Transaction cannot be null.");
                int retVal = await _MySqlDatabaseManager.Delete(rec, _dbTransaction);
                if (transactionCheck && retVal > 0)
                {
                    CommitTransaction();
                    return await Task.FromResult(retVal);
                }
                throw new Exception("Silme işlemi gerçekleştirilemedi");
            }
            catch
            {
                if (transactionCheck) RollbackTransaction();
                throw;
            }
        }

        public Task<List<T>> GetAll()
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("MySqlAdoBaseRepository\nTransaction cannot be null.");
                var returnRecList = _MySqlDatabaseManager.GetAll(_dbTransaction, _selectColums, _joinTable);
                if (transactionCheck) CommitTransaction();
                return returnRecList;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public Task<List<T>> GetModifieds()
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("MySqlAdoBaseRepository\nTransaction cannot be null.");
                var returnRecList = _MySqlDatabaseManager.GetModifieds(_dbTransaction, _selectColums, _joinTable);
                if (transactionCheck) CommitTransaction();
                return returnRecList;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public Task<T> Find(long id)
        {
            bool transactionCheck = false;

            if (_dbTransaction == null)
            {
                BeginTransaction();
                transactionCheck = true;
            }
            try
            {
                if (_dbTransaction == null)
                    throw new ArgumentException("MySqlAdoBaseRepository\nTransaction cannot be null.");
                var returnRec = _MySqlDatabaseManager.GetById(id, _dbTransaction, _selectColums, _joinTable);
                if (transactionCheck) CommitTransaction();
                return returnRec;
            }
            catch (Exception ex)
            {
                if (transactionCheck) RollbackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public void BeginTransaction()
        {
            _dbConnection = new MySqlConnection(_Login.ConnStr);
            if (_dbConnection.State == ConnectionState.Closed) _dbConnection.Open();
            _dbTransaction = _dbConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_dbTransaction == null)
                throw new ArgumentException("Transaction cannot be null.");
            if (_dbConnection == null)
                throw new ArgumentException("Connection cannot be null.");
            _dbTransaction.Commit();
            _dbConnection.Close();
            _dbConnection.Dispose();
        }

        public void RollbackTransaction()
        {
            if (_dbTransaction == null)
                throw new ArgumentException("Transaction cannot be null.");
            if (_dbConnection == null)
                throw new ArgumentException("Connection cannot be null.");
            _dbTransaction.Rollback();
            _dbConnection.Close();
            _dbConnection.Dispose();
            _dbTransaction = null;
        }
    }
}
