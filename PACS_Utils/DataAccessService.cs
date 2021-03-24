using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PACS_Utils
{
    public class DataAccessService
    {
        // TODO: guardar la connection string en el app.config
        private static readonly string ConnectionString =
            @"Data Source=den1.mssql7.gear.host;Initial Catalog=muchocodigodtb;User ID=muchocodigodtb;Password=[MuchoCodigo1T]";

        private static readonly Configuration Config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private SqlConnection _conn;

        #region methods

        public void EncryptConnString()
        {
            Config.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
        }

        private void ConnectDb()
        {
            // TODO: connection to the database with the specified connectionString stored encrypted in the app.config file
            try
            {
                _conn = new SqlConnection(ConnectionString);
            }
            catch (Exception e)
            {
                // if an exception is thrown and the connection is up, this will close it
                _conn?.Close();

                // to be changed with a invisible label on the Form that gets displayed when an error happens
                MessageBox.Show($"La connexió a la base de dades no s'ha pogut realitzar. Excepció {e}",
                    "Error no fatal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RunSafeQuery(string query, Dictionary<string, dynamic> parameters)
        {
            try
            {
                ConnectDb();

                var cm = new SqlCommand(query, _conn);

                foreach (var param in parameters)
                    cm.Parameters.AddWithValue(param.Key, param.Value);

                _conn.Open();

                cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessage(e, "L'execució de la consulta a la base de dades no s'ha executat correctament", null);
            }
            finally
            {
                _conn?.Close();
            }
        }

        public DataSet GetTable(string table)
        {
            DataSet ds;
            try
            {
                ds = GetByQuery($"SELECT * FROM {table}");
                ds.Tables[0].TableName = table;
            }
            catch (Exception e)
            {
                ErrorMessage(e, @"La petició de dades d'una taula no s'ha pogut realitzar", null);
                ds = null;
            }
            finally
            {
                _conn?.Close();
            }

            return ds;
        }

        public DataSet GetByQuery(string query)
        {
            DataSet ds;

            try
            {
                // first we call the connectDB method so we now have our public variable conn initialized
                ConnectDb();

                // we initialize too our DataSet
                ds = new DataSet();

                // we initialize the adapter that provides communication between the DataSet and the SQL Database
                var adapter = new SqlDataAdapter(query, _conn);

                _conn.Open();

                adapter.Fill(ds);
            }
            catch (Exception e)
            {
                // to be changed with a invisible label on the Form that gets displayed when an error happens, although this does the work
                ErrorMessage(e, "La presa de dades ha fallat", null);
                ds = null;
            }
            finally
            {
                // if no exception is thrown the connection will close. Notice it is nearly instant, so it's pretty hard to mess up something
                _conn?.Close();
            }

            return ds;
        }

        public DataSet GetByQuery(string query, Dictionary<string, dynamic> parameters)
        {
            DataSet ds;

            try
            {
                // first we call the connectDB method so we now have our public variable conn initialized
                ConnectDb();

                // we initialize too our DataSet
                ds = new DataSet();

                // we initialize the adapter that provides communication between the DataSet and the SQL Database
                var adapter = new SqlDataAdapter(query, _conn);

                foreach (var param in parameters)
                    adapter.SelectCommand.Parameters.AddWithValue(param.Key, param.Value);

                _conn.Open();

                adapter.Fill(ds);
            }
            catch (Exception e)
            {
                // to be changed with a invisible label on the Form that gets displayed when an error happens, although this does the work
                ErrorMessage(e, "La presa de dades ha fallat", null);
                ds = null;
            }
            finally
            {
                // if no exception is thrown the connection will close. Notice it is nearly instant, so it's pretty hard to mess up something
                _conn?.Close();
            }

            return ds;
        }

        public DataSet GetByQuery(string query, string dataTableName)
        {
            DataSet ds;
            try
            {
                // we first get our DataSet using the other method
                ds = GetByQuery(query);

                // we create a new DataTable in which we add the table of the first DataSet, and then we change the TableName
                var newDt = ds.Tables[0];
                newDt.TableName = dataTableName;

                // we add the DataTable to a new DataSet
                ds = new DataSet();
                ds.Tables.Add(newDt);
            }
            catch (Exception e)
            {
                // if an exception is thrown, it will show an error message and return null
                ErrorMessage(e, null, null);
                ds = null;
            }
            finally
            {
                // we use the null propagation value to close the connection to the DB, only if it exists
                _conn?.Close();
            }

            // the DataSet is returned
            return ds;
        }

        // runs the query that is sent to it by params on the database
        public void RunQuery(string query)
        {
            try
            {
                ConnectDb();

                var cm = new SqlCommand(query, _conn);

                _conn.Open();

                cm.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ErrorMessage(e, "L'execució de la consulta a la base de dades no s'ha executat correctament", null);
            }
            finally
            {
                _conn?.Close();
            }
        }

        public int UpdateDb(string query, DataSet newDs)
        {
            var changes = 0;
            try
            {
                ConnectDb();
                var adapter = new SqlDataAdapter(query, _conn);
                var cmdBuilder = new SqlCommandBuilder(adapter);

                if (newDs.HasChanges()) changes = adapter.Update(newDs.Tables[0]);
            }
            catch (Exception e)
            {
                ErrorMessage(e, "L'execució de la consulta a la base de dades no s'ha executat correctament", null);
            }
            finally
            {
                _conn?.Close();
            }

            return changes;
        }

        private static string GetColumnNames(DataTable dt)
        {
            var ls = new string[dt.Columns.Count];

            for (var i = 0; i < dt.Columns.Count; i++) ls[i] = dt.Columns[i].ColumnName;

            return string.Join(", ", ls);
        }

        private static string GetValues(DataRow dr)
        {
            var finalStr = "";
            var index = 0;

            foreach (var obj in dr.ItemArray)
            {
                var str = !Regex.IsMatch(obj.ToString(), @"^\d+$") ? $"'{obj}'" : $"{obj}";
                if (index != dr.ItemArray.Length - 1) str = $"{str}, ";

                finalStr = $"{finalStr}{str}";
                index++;
            }

            return finalStr;
        }

        public string CreateChainedTransaction(List<string> queries)
        {
            return $@"BEGIN TRANSACTION; {string.Join(" ", queries)} COMMIT TRANSACTION;";
        }


        private void ErrorMessage(Exception e, string message, string title)
        {
            MessageBox.Show($"{message ?? "Error"}: Excepció {e.ToString() ?? "Error no fatal"}", title,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public int Getid(string nombretabla, string campoid, string campodesc, string valoredi)
        {
            _conn = new SqlConnection(ConnectionString);

            _conn.Open();

            var query = "SELECT " + campoid + " FROM [" + nombretabla + "] WHERE " + campodesc + "= '" + valoredi +
                        "'";

            var command = new SqlCommand(query, _conn);

            //command.CommandType = CommandType.Text;

            var id = Convert.ToInt32(command.ExecuteScalar());

            _conn.Close();
            _conn.Dispose();

            return id;
        }

        #endregion
    }
}