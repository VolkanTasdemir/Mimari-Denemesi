using MvT.Dal.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MvT.Bll.System
{
    public class MvTSystem
    {
    }

    public class MvTSystem<T> where T : class
    {
        public DataTable ConvertListToDataTable(List<T> list)
        {
            DataTable dataTable = new();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                dataTable.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
            }

            foreach (T item in list)
            {
                DataRow dataRow = dataTable.NewRow();

                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    dataRow[property.Name] = property.GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }
    }

    
}
