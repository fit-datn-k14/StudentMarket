using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace StudentMarket.Common.Utilities
{
    public static class EntityUtilities
    {
        #region Method

        public static string GetTableName<T>()
        {
            string tableName = typeof(T).Name;
            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if (tableAttributes.Count() > 0)
            {
                tableName = tableAttributes.First().Name;
            }
            return tableName;
        } 

        #endregion
    }
}
