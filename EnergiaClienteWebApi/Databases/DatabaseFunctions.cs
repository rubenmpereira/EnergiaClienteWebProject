using System.Data;
using EnergiaClienteWebApi.Databases.Interfaces;
using Microsoft.Data.SqlClient;

namespace EnergiaClienteWebApi.Databases;

public class DatabaseFunctions : IDatabaseFunctions
{

    private static SqlConnection connection = new SqlConnection("Data Source=192.168.1.8,1433;Initial Catalog=EnergiaClienteDados;User ID=sa;Password=1Secure*Password1;TrustServerCertificate=True");

    public DataRowCollection RunSelectProcedure(string procedure, List<SqlParameter> parameters)
    {
        var dataAdapter = new SqlDataAdapter(procedure, connection);
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        dataAdapter.SelectCommand.Parameters.AddRange(parameters.ToArray());
        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet.Tables[0].Rows;
    }

    public DataRowCollection RunSelectProcedure(string procedure)
    {
        var dataAdapter = new SqlDataAdapter(procedure, connection);
        dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        var dataSet = new DataSet();
        dataAdapter.Fill(dataSet);

        return dataSet.Tables[0].Rows;
    }

    public bool RunInsertProcedure(string procedure, List<SqlParameter> parameters)
    {
        var command = new SqlCommand(procedure, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.AddRange(parameters.ToArray());

        try
        {
            connection.Open();
            return command.ExecuteNonQuery() > 0 ? true : false;
        }
        catch (Exception ex)
        {
            var x = ex;
            return false;
        }
        finally
        {
            connection.Close();
        }
    }

    public T? GetParam<T>(object value)
    {
        object x = value != null ? value : "";

        string? param = x.ToString();

        T? result;

        try
        {
            result = (T?)Convert.ChangeType(param, typeof(T));
        }
        catch
        {
            result = default(T);
        }

        return result != null ? result : default(T);
    }

}