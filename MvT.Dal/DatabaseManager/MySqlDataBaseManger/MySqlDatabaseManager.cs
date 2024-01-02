using MvT.Dal.Entities;
using MvT.Entities.Enums;
using MvT.Entities.Interface;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using System.Text;

namespace MvT.Dal.DatabaseManager.MySqlDataBaseManger
{
    public class MySqlDatabaseManager<T> where T : class, IDbEntity
    {
        private readonly string entityName = typeof(T).Name;

        public Task<T> Insert(T entity, IDbTransaction trans)
        {
            Int64 recID = 0;
            if (entity == null)
                throw new ArgumentException("MySqlDatabaseManager\nEntity cannot be null for insert.");
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for insert.");
            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert."); //== null ? throw new ArgumentException("Connection cannot be null for insert.") : trans.Connection;
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();

                string insertQuery = $"INSERT INTO {entityName} ({string.Join(", ", properties.Select(p => p.Name))}) VALUES " +
                                     $"({string.Join(", ", properties.Select(p => $"@{p.Name}"))})" +
                                     $";SELECT LAST_INSERT_ID();";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(insertQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter;

                foreach (PropertyInfo property in properties)
                {

                    MySqlDbType propertyDbType = GetSqlType(property.PropertyType);
                    if (propertyDbType == 0) throw new ArgumentException($"MySqlDatabaseManager\nUnsupported type: {property.PropertyType}");
                    parameter = new MySqlParameter(parameterName: $"@{property.Name}", dbType: propertyDbType) { Value = property.GetValue(entity) ?? (object)DBNull.Value };
                    sqlCommand.Parameters.Add(parameter);
                }
                recID = Convert.ToInt64(sqlCommand.ExecuteScalar());
                entity.Id = recID;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
            return Task.FromResult(entity);
        }

        public Task<List<T>> BulkInsert(List<T> listRec, IDbTransaction trans)
        {
            try
            {
                foreach (T rec in listRec)
                    rec.Id = Insert(rec, trans).Id;
                return Task.FromResult(listRec);
            }
            finally { }
        }

        public Task<T> Update(T entity, IDbTransaction trans)
        {
            if (entity == null)
                throw new ArgumentException("MySqlDatabaseManager\nEntity cannot be null for update.");
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for update.");
            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert.");
            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                PropertyInfo[] properties = entity.GetType().GetProperties();

                string updateQuery = $"UPDATE {entityName} SET {string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"{p.Name} = @{p.Name}"))} WHERE Id = @Id";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(updateQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter;

                foreach (PropertyInfo property in properties)
                {
                    entity.DateStatus = (short)DataStatus.Updated;
                    entity.ModifiedDate = DateTime.Now;

                    MySqlDbType propertyDbType = GetSqlType(property.PropertyType);
                    if (propertyDbType == 0) throw new ArgumentException($"MySqlDatabaseManager\nUnsupported type: {property.PropertyType}");
                    parameter = new MySqlParameter(parameterName: $"@{property.Name}", dbType: propertyDbType) { Value = property.GetValue(entity) ?? (object)DBNull.Value };
                    sqlCommand.Parameters.Add(parameter);
                }

                int returnValue = sqlCommand.ExecuteNonQuery();
                if (returnValue > 0)
                    return Task.FromResult(entity);
                throw new ArgumentException("MySqlDatabaseManager\nUpdate could not be performed.");
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public Task<List<T>> BulkUpdate(List<T> listRec, IDbTransaction trans)
        {
            try
            {
                foreach (T rec in listRec)
                    Update(rec, trans);
                return Task.FromResult(listRec);
            }
            finally { }
        }

        public Task<int> Delete(T entity, IDbTransaction trans)
        {
            int returnValue = 0;
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert.");

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string deleteQuery = $"DELETE FROM {entityName} WHERE Id = @Id";

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(deleteQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter = new MySqlParameter(parameterName: "@Id", dbType: MySqlDbType.Int64) { Value = ((IDbEntity)entity).Id };

                sqlCommand.Parameters.Add(parameter);
                returnValue = sqlCommand.ExecuteNonQuery();
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }

            return Task.FromResult(returnValue);
        }

        //public void BulkDelete(List<T> listRecod, IDbTransaction trans)
        //{
        //    try
        //    {
        //        foreach (T rec in listRecod)
        //        {
        //            Delete(recId, trans);
        //        }
        //    }
        //    finally { }
        //}

        public Task<List<T>> GetAll(IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert."); /*== null ? throw new ArgumentException("Connection cannot be null for insert.") : trans.Connection;*/

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string getAllQuery = $"SELECT {selectColums} FROM {entityName} {joinTable}";

                List<T> returnList = new();
                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(getAllQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    T rec = Activator.CreateInstance<T>();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        if (dataReader.IsDBNull(i)) continue;
                        PropertyInfo property = typeof(T).GetProperty(dataReader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException("PropertyInfocannot be null.\n"); ;
                        property.SetValue(rec, Convert.ChangeType(dataReader.GetValue(i), property.PropertyType));
                    }

                    returnList.Add(rec);
                }
                return Task.FromResult(returnList);

                //IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                //dataAdapter.SelectCommand = sqlCommand;
                //DataSet dataSet = new();
                //dataAdapter.Fill(dataSet);
                //return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public Task<List<T>> GetModifieds(IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert."); /*== null ? throw new ArgumentException("Connection cannot be null for insert.") : trans.Connection;*/

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string getAllQuery = $"SELECT {selectColums} FROM {entityName} {joinTable} WHERE CratedDate IS NOT NULL";

                List<T> returnList = new();
                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(getAllQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataReader dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    T rec = Activator.CreateInstance<T>();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        if (dataReader.IsDBNull(i)) continue;
                        PropertyInfo property = typeof(T).GetProperty(dataReader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException("PropertyInfocannot be null.\n"); ;
                        property.SetValue(rec, Convert.ChangeType(dataReader.GetValue(i), property.PropertyType));
                    }

                    returnList.Add(rec);
                }
                return Task.FromResult(returnList);

                //IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                //dataAdapter.SelectCommand = sqlCommand;
                //DataSet dataSet = new();
                //dataAdapter.Fill(dataSet);
                //return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public Task<T> GetById(Int64 Id, IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert.");

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
                string GetByIdQuery = $"SELECT {selectColums} FROM {entityName} {joinTable} WHERE Id = @Id";
                List<T> returnList = new();

                IDbCommand sqlCommand = (IDbCommand)new MySqlCommand(GetByIdQuery, (MySqlConnection)dbConnection, (MySqlTransaction)trans);
                IDataParameter parameter = new MySqlParameter(parameterName: "@Id", dbType: MySqlDbType.Int64) { Value = Id };
                sqlCommand.Parameters.Add(parameter);

                IDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    T rec = Activator.CreateInstance<T>();

                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        if (dataReader.IsDBNull(i)) continue;
                        PropertyInfo property = typeof(T).GetProperty(dataReader.GetName(i), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ?? throw new ArgumentException("PropertyInfocannot be null.\n"); ;
                        property.SetValue(rec, Convert.ChangeType(dataReader.GetValue(i), property.PropertyType));
                    }

                    returnList.Add(rec);
                }
                if (returnList.Count != 1)
                    throw new ArgumentException("MySqlDatabaseManager\nCannot have more than one record.");
                return Task.FromResult(returnList[0]);

                //IDbDataAdapter dataAdapter = (IDbDataAdapter)new MySqlDataAdapter();
                //dataAdapter.SelectCommand = sqlCommand;
                //DataSet dataSet = new();
                //dataAdapter.Fill(dataSet);
                //return dataSet;
            }
            finally
            {
                if (trans == null) dbConnection.Close();
            }
        }

        public DataSet GetByFilter(List<Filter> filters, IDbTransaction trans, string selectColums = "*", string joinTable = "")
        {
            if (trans == null)
                throw new ArgumentException("MySqlDatabaseManager\nTransaction cannot be null for delete.");

            IDbConnection dbConnection = trans.Connection ?? throw new ArgumentException("MySqlDatabaseManager\nConnection cannot be null for insert."); //== null ? throw new ArgumentException("Connection cannot be null for insert.") : trans.Connection;

            if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

            try
            {
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
            StringBuilder whereClause = new("WHERE ");
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
