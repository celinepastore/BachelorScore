using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctionTest1
{
    public static class Function1
    {
        [Function("Function1")]
        public static void Run([TimerTrigger("0 */15 * * * *"
            #if DEBUG
                        , RunOnStartup=true
            #endif
                        )]MyInfo myTimer, FunctionContext context)
        {
            var logger = context.GetLogger("Function1");
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string connectionString = "Server=tcp:bach-world-1.database.windows.net,1433;Initial Catalog=BachBase;Persist Security Info=False;User ID=BachAdmin;Password=P@$$word1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("dbo.sp_test", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                cmd.Parameters.Add(new SqlParameter("@id", 1));
                cmd.Parameters.Add(new SqlParameter("@Name", "Jason"));
                cmd.ExecuteNonQuery();
            }


            logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
