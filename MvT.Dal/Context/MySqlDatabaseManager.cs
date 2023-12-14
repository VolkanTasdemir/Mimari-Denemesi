using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using MvT.Dal.Entities;
using MvT.Entities.Interface;
using MySql.Data.MySqlClient;
namespace MvT.Dal.Context
{
    public class MySqlDatabaseManager<T> where T : class, IDbEntity
    {
        private readonly Login _login;
        public MySqlDatabaseManager(Login login)
        {
            _login = login;
        }
        public int Insert(T entity, IDbTransaction trans)
        {
            int returnValue = 0;
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for insert.");
            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();
                string entityName = entity.GetType().Name;

                string insertQuery = $"INSERT INTO {entityName} ({string.Join(", ", properties.Select(p => p.Name))}) VALUES " +
                                     $"({string.Join(", ", properties.Select(p => $"@{p.Name}"))})";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(insertQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter;

                foreach (PropertyInfo property in properties)
                {

                    MySqlDbType propertyDbType = GetSqlType(property.PropertyType);
                    if (propertyDbType == 0) throw new ArgumentException($"Unsupported type: {property.PropertyType}");
                    parameter = new MySqlParameter(parameterName: $"@{property.Name}", dbType: propertyDbType) { Value = property.GetValue(entity) ?? (object)DBNull.Value };
                    sqlCommand.Parameters.Add(parameter);
                }
                returnValue = sqlCommand.ExecuteNonQuery();

            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
            return returnValue;
        }

        public void BulkInsert(List<T> listRecod, IDbTransaction trans)
        {
            try
            {
                foreach (T rec in listRecod)
                {
                    Insert(rec, trans);
                }
            }
            finally { }
        }

        public int Update(T entity, IDbTransaction trans)
        {
            int returnValue = 0;
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for update.");

            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();
                string entityName = entity.GetType().Name;

                string updateQuery = $"UPDATE {entityName} SET {string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"{p.Name} = @{p.Name}"))} WHERE Id = @Id";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(updateQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter;

                foreach (PropertyInfo property in properties)
                {
                    MySqlDbType propertyDbType = GetSqlType(property.PropertyType);
                    if (propertyDbType == 0) throw new ArgumentException($"Unsupported type: {property.PropertyType}");
                    parameter = new MySqlParameter(parameterName: $"@{property.Name}", dbType: propertyDbType) { Value = property.GetValue(entity) ?? (object)DBNull.Value };
                    sqlCommand.Parameters.Add(parameter);
                }

                returnValue = sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }

            return returnValue;
        }

        public void BulkUpdate(List<T> listRecod, IDbTransaction trans)
        {
            try
            {
                foreach (T rec in listRecod)
                {
                    Update(rec, trans);
                }
            }
            finally { }
        }

        public int Delete(T entity, IDbTransaction trans)
        {
            int returnValue = 0;
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string entityName = typeof(T).Name;
                string deleteQuery = $"DELETE FROM {entityName} WHERE Id = @Id";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(deleteQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter = new MySqlParameter(parameterName: "@Id", dbType: MySqlDbType.Int64) { Value = entity.Id };

                sqlCommand.Parameters.Add(parameter);
                returnValue = sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }

            return returnValue;
        }

        public void BulkDelete(List<T> listRecod, IDbTransaction trans)
        {
            try
            {
                foreach (T rec in listRecod)
                {
                    Delete(rec, trans);
                }
            }
            finally { }
        }

        public DataSet GetAll(T entity, IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string entityName = entity.GetType().Name;
                string getAllQuery = $"SELECT {selectColums} FROM {entityName} {joinTable}";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(getAllQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);

                IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                dataAdapter.SelectCommand = sqlCommand;
                DataSet dataSet = new();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public DataSet GetById(T entity, IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string entityName = typeof(T).Name;
                string GetByIdQuery = $"SELECT {selectColums} FROM {entityName} {joinTable} WHERE Id = @Id";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(GetByIdQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter = new MySqlParameter(parameterName: "@Id", dbType: MySqlDbType.Int64) { Value = entity.Id };
                sqlCommand.Parameters.Add(parameter);


                IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                dataAdapter.SelectCommand = sqlCommand;
                DataSet dataSet = new();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public DataSet GetByFilter(T entity, List<Filter> filters, IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("Transaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? new MySqlConnection(_login.ConnStr);

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string entityName = entity.GetType().Name;
                string getGetByFilter = GetByDynamicFilterQuery(entityName, filters, selectColums, joinTable);

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(getGetByFilter, (MySqlConnection)dbConnection, (MySqlTransaction)trans);

                IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                dataAdapter.SelectCommand = sqlCommand;
                DataSet dataSet = new();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        private static string GetByDynamicFilterQuery(string tableName, List<Filter> filters, string selectColums = "*", string joinTable = "")
        {
            StringBuilder whereClause = new ("WHERE ");
            bool isFirst = true;

            foreach (var filter in filters)
            {
                if (!isFirst)
                {
                    whereClause.Append($" {filter.LogicOperator} ");
                }

                if (filter.IsNull)
                {
                    whereClause.Append($"{filter.Field} IS NULL");
                }
                else if (filter.IsNotNull)
                {
                    whereClause.Append($"{filter.Field} IS NOT NULL");
                }
                else if (filter.IsLike)
                {
                    whereClause.Append($"{filter.Field} {filter.Operator} @{filter.Field}");
                }
                else if (filter.IsBetween)
                {
                    whereClause.Append($"{filter.Field} {filter.Operator} @{filter.Field} AND @{filter.Field}2");
                }
                else
                {
                    whereClause.Append($"{filter.Field} {filter.Operator} @{filter.Field}");
                }

                isFirst = false;
            }

            var query = $"SELECT {selectColums} FROM {tableName} {joinTable} {whereClause}";
            return query;
        }

        private static MySqlDbType GetSqlType(System.Type type)
        {
            MySqlDbType propertyType = 0;
            switch (type.Name)
            {
                case "string":
                    propertyType = MySqlDbType.String;
                    break;
                case "short":
                    propertyType = MySqlDbType.Int16;
                    break;
                case "int":
                    propertyType = MySqlDbType.Int32;
                    break;
                case "long":
                    propertyType = MySqlDbType.Int64;
                    break;
                case "DateTime":
                    propertyType = MySqlDbType.DateTime;
                    break;
                case "bool":
                    propertyType = MySqlDbType.Byte;
                    break;
            }
            return propertyType;
            //denemeden sonra silinecek
            // if (type == typeof(string))
            //     return DbType.AnsiString;
            // else if (type == typeof(int))
            //     return DbType.Int32;
            // // Diğer türleri ekleyebilirsiniz
        }

    }
}
