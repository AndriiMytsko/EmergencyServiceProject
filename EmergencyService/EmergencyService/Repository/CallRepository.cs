using EmergencyService.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EmergencyService.Repository
{
    class CallRepository : ICallRepository
    {
        private const string GetAllCalls = "SELECT * FROM Call";

        private const string AddCall = "INSERT INTO Call (UserName, OperatorName, DepartmentName, State)"
            + "VALUES (@userName, @operatorName, @departmentName, @state); SELECT SCOPE_IDENTITY()";

        private const string UpdateCall = "UPDATE Call SET State = @state WHERE Id=@id";

        private readonly string _connectionString;

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private SqlCommand CreateSqlCommand(string command, SqlConnection connection)
        {
            return new SqlCommand(command, connection);
        }

        public CallRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IList<Call> GetAll()
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (var sqlCommand = CreateSqlCommand(GetAllCalls, connection))
                {
                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        var calls = new List<Call>();
                        if (!sqlDataReader.HasRows)
                        {
                            return calls;
                        }

                        while (sqlDataReader.Read())
                        {
                            var call = new Call
                            {
                                Id = (int)sqlDataReader["Id"],
                                UserName = (string)sqlDataReader["UserName"],
                                OperatorName = (string)sqlDataReader["OperatorName"],
                                DepartmentName = (string)sqlDataReader["DepartmentName"],
                                State = (string)sqlDataReader["State"]
                            };

                            calls.Add(call);
                        }

                        return calls;
                    }
                }
            }
        }

        public int Add(string userName, string operatorName, string departmentName, string state)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (var sqlCommand = CreateSqlCommand(AddCall, connection))
                {
                    AddParameter("@userName", userName, sqlCommand);
                    AddParameter("@operatorName", operatorName, sqlCommand);
                    AddParameter("@departmentName", departmentName, sqlCommand);
                    AddParameter("@state", state, sqlCommand);

                    var idResult = sqlCommand.ExecuteScalar();
                    return Convert.ToInt32(idResult);
                }
            }
        }

        public void UpdateState(int callId, string state)
        {
            using (SqlConnection connection = CreateConnection())
            {
                connection.Open();
                using (var sqlCommand = CreateSqlCommand(UpdateCall, connection))
                {
                    sqlCommand.Parameters.AddWithValue("@state", state);
                    sqlCommand.Parameters.AddWithValue("@id", callId);

                    sqlCommand.ExecuteNonQuery();
                };
            }
        }

        private void AddParameter(string name, string value, SqlCommand command)
        {
            var param = new SqlParameter(name, value);
            command.Parameters.Add(param);
        }
    }
}