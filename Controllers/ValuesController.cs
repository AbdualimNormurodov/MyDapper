using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MyDapper.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController] 
    public class ValuesController : ControllerBase
    {
        private string connectionString = WebApplication.CreateBuilder().Configuration.GetConnectionString("DefaultConnectionString");
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            using(var connection=new SqlConnection(connectionString))
            {
                string query = "Select *from Employee";
                IEnumerable<Employee> employees = connection.Query<Employee>(query);
                return Ok(employees);
            }
        }
        [HttpPost]
        public IActionResult CreateUser(MessageDTO messageDTO) {
            using(var connection=new SqlConnection(connectionString))
            {
                string query = $"Insert into Message Values('{messageDTO.UserName}','{messageDTO.Messenger}')";
                connection.Execute(query);
                return Ok("Created");
            }
        }
        [HttpGet]
        public async ValueTask<IActionResult> GetAllMultpleQueryUsers()
        {
            using(var connection=new SqlConnection(connectionString))
            {
                string query = @"Select *from Employee;
                                    Select *from Message";
                var employees=await connection.QueryMultipleAsync(query);
                var firstTable=employees.ReadAsync<Employee>().Result;
                var SecondTable=employees.ReadAsync<MessageDTO>().Result;
                return Ok(firstTable);
            }
        }
    }
}
