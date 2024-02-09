using System.Data;
using Microsoft.Data.SqlClient;

namespace EnergiaClienteWebApi.Databases.Interfaces;

public interface IDatabaseFunctions
{
    public DataRowCollection RunSelectProcedure(string procedure, List<SqlParameter> parameters);
    public DataRowCollection RunSelectProcedure(string procedure);
    public bool RunInsertProcedure(string procedure, List<SqlParameter> parameters);
    public T? GetParam<T>(object value);
}
