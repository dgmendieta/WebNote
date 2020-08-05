/// Selim Özbudak
/*
 *  24.11.2008 SÖ  - SqlDataAccess is created.
 *  03.12.2008 SÖ  - Connectionstring is made singleton.
 *  12.12.2008 SÖ  - ExecuteNonQuery has been changed. 
 *                   (now it is being use for insert, update, delete queries.)
 *  12.12.2008 SÖ  - ExecuteStoredProcedure has been created.                
 */
///

using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

/// <summary>
/// SqlDataAccess is being used for accessing MSSQL database quickly and easily. 
/// Requires a connection string that is named MsSql defined on web.config file. This connection string is used as default. 
/// For using different connection strings you should pass the name of the connection string as a parameter with methods.
/// </summary>
public class SqlDataAccess
{
    // Default connection string. a connection named MsSql should be defined in web.config file.
    public const string CONNECTION_STRING_NAME = @"Data Source=.;Initial Catalog=NoteSoftSys;Integrated Security=True;";
    
    public static string ConnectionString { get; set; }

    private SqlConnection conn;
    /// <summary>
    /// Brings a SqlCommand object to be able to add some parameters in it. After you send this to Execute method.
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public SqlDataAccess()
    {
        SqlDataAccess.ConnectionString = @"Data Source=.;Initial Catalog=NoteSoftSys;Integrated Security=True;";

        conn = new SqlConnection(ConnectionString);
        conn.Open();
    }

    /// <summary>
    /// Datatable Döndür
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public DataTable Execute(SqlCommand command)
    {
        DataTable dt = new DataTable();
        command.Connection.Open();
        //command.ExecuteNonQuery();
        dt.Load(command.ExecuteReader());
        command.Connection.Close();
        return dt;
    }

    /// <summary>
    /// Ejecuta una sentencia en la BD, es decir algo que afecta cierta cantidad de tuplas, pero no algo que tiene que retornar datos
    /// </summary>
    /// <param name="sentenciaSQL">Sentencia a ser ejecutada</param>
    /// <param name="params_values">Parámetros de la consulta</param>
    /// <returns>Indica si se afectaron tuplas en la consulta o no</returns>
    public bool ExecuteSentenceParameterized(string sentenciaSQL, Dictionary<string, object> params_values)
    {
        if ((conn != null) && (conn.State == ConnectionState.Closed))
        {
            throw new Exception("La base de datos se encuentra desconectada");
        }
        else
        {
            int result = 0;
            // Transacción a ser ejecutada en la BD
            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.Connection = conn;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = sentenciaSQL;

            // Se recorre la lista de <parametro,valor> agregando cada uno a la sentencia
            foreach (KeyValuePair<string, object> pair in params_values)
            {
                if (pair.Value.GetType() == typeof(int))
                {
                    sqlCmd.Parameters.Add(pair.Key, SqlDbType.Int, 4).Value = pair.Value;
                }
                else if (pair.Value.GetType() == typeof(byte[]))
                {
                    sqlCmd.Parameters.Add(pair.Key, SqlDbType.Binary).Value = pair.Value;
                }
                else if (pair.Value.GetType() == typeof(double))
                {
                    sqlCmd.Parameters.Add(pair.Key, SqlDbType.Float).Value = pair.Value;
                }
                else if (pair.Value.GetType() == typeof(DateTime))
                {
                    sqlCmd.Parameters.Add(pair.Key, SqlDbType.DateTime).Value = pair.Value;
                }
                else
                {
                    sqlCmd.Parameters.Add(pair.Key, SqlDbType.VarChar, 6000).Value = pair.Value;
                }
            }
            // Ejecuta la consulta y retorna la cantidad de tuplas afectadas
            result = sqlCmd.ExecuteNonQuery();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    /// <summary>
    /// Ejecuta una consulta que retorna algo, generalmente un select. Se setea el timeout en 260
    /// </summary>
    /// <param name="sentenciaSQL">Sentencia a ser ejecutada</param>
    /// <param name="params_values">Parámetros de la consulta</param>
    /// <returns>DataSet con el resultado de lo ejecutado</returns>
    public DataSet ExecuteQueryParameterized(string sentenciaSQL, Dictionary<string, object> params_values, int varCharLength = 6000)
    {
            if ((conn != null) && (conn.State == ConnectionState.Closed))
            {
                throw new Exception("La base de datos se encuentra desconectada");
            }
            else
            {
                // Para realizar una consulta que retorna DataSet
                SqlDataAdapter da = default(SqlDataAdapter);
                DataSet ds = new DataSet();

                da = new SqlDataAdapter(sentenciaSQL, conn);

                // Se recorre la lista de <parametro,valor> agregando cada uno al adapter
                foreach (KeyValuePair<string, object> pair in params_values)
                {
                    SqlParameter param;
                if (pair.Value.GetType() == typeof(int))
                {
                    param = new SqlParameter(pair.Key, SqlDbType.Int, 4);
                }
                else if (pair.Value.GetType() == typeof(byte[]))
                {
                    param = new SqlParameter(pair.Key, SqlDbType.Binary);
                }
                else if (pair.Value.GetType() == typeof(double))
                {
                    param = new SqlParameter(pair.Key, SqlDbType.Float);
                }
                else
                {
                    param = new SqlParameter(pair.Key, SqlDbType.VarChar, varCharLength);
                }

                param.Direction = ParameterDirection.Input;

                    param.Value = pair.Value;
                    da.SelectCommand.Parameters.Add(param);
                }

                da.SelectCommand.CommandTimeout = 260;
                da.Fill(ds);
                return ds;
            }

    }
}
