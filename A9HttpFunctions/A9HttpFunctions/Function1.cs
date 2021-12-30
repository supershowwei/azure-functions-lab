using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace A9HttpFunctions
{
    public static class Function1
    {
        [FunctionName("MemberFunction")]
        public static async Task<IActionResult> RunMemberFunction(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "member/{id:int}")] HttpRequest req,
            int id,
            ILogger log)
        {
            var sql = @"
SELECT
    m.Id
   ,m.[Name]
FROM Member m WITH (NOLOCK)
WHERE m.Id = @Id";

            Member member;

            using (var db = new SqlConnection("Server=tcp:my-first-a9-db-server.database.windows.net,1433;Initial Catalog=my-first-a9-db;Persist Security Info=False;User ID=test_sa;Password=D3amM8GssSy74W3G;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                // Dapper required.
                member = await db.QuerySingleOrDefaultAsync<Member>(sql, new { Id = id });
            }

            return new JsonResult(member);
        }
    }

    public class Member
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}