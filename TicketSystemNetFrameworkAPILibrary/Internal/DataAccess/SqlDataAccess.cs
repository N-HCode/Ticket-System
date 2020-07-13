using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//This uses Dapper. Is a mirco ORM (Object Relation Mapper). allows you to talk to database to get information
//and then map it to an object. Works really well with StoredProcedure

namespace TicketSystemNetFrameworkAPILibrary.Internal.DataAccess
{
    internal class SqlDataAccess : IDisposable
    {
        public string GetConnectionString(string name)
        {
            //look in the system configuration for a ConnectionString with a matching name
            //and then return that connection string.
            //Since this is a library there is no config file. It has to be ran through a web or program
            //This will use the web config for the API
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public List<T> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using(IDbConnection connection = new SqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(storedProcedure, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<T>(string storedProcedure, T parameters, string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Execute(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure);
            }
        }

        private IDbConnection _connection;
        private IDbTransaction _transaction;

        public void StartTransaction(string connectionStringName)
        {
            string connectionString = GetConnectionString(connectionStringName);

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public List<T> LoadDataInTransaction<T, U>(string storedProcedure, U parameters)
        {
            List<T> rows = _connection.Query<T>(storedProcedure, parameters,
                commandType: CommandType.StoredProcedure, transaction: _transaction).ToList();

            return rows;
        }  

        public void SaveDataInTransaction<T>(string storedProcedure, T parameters)
        {
                _connection.Execute(storedProcedure, parameters,
                   commandType: CommandType.StoredProcedure, transaction: _transaction);
        }

        //apply changes to the database
        public void CommitTransaction()
        {
            _transaction?.Commit();
            _connection?.Close();
        }
        //Rollback all the changes
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }

        //This will run after the end of the using statement even if we do not tell it 
        //to expicitly. Since, we assume that if it
        //reach the end of the using statement, that everything worked fine, we use the
        //commitTransaction method. Remember the using statement is based on Dapper
        public void Dispose()
        {
            CommitTransaction();
        }
    }
}
