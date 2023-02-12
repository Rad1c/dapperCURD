using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCRUD.Controllers
{
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("/employees")]
        public async Task<ActionResult<List<Employee>>> GetEmoloyees()
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            var employees = await connection.QueryAsync<Employee>("SELECT * from employee");

            return Ok(employees);
        }

        [HttpGet("/employee/{employeeId}")]
        public async Task<ActionResult<List<Employee>>> GetEmoloyeeById(int employeeId)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            var employee = await connection.QueryFirstAsync<Employee>("SELECT * from employee where id=@id",
                new {id = employeeId});

            return Ok(employee);
        }

        [HttpPost("/employee")]
        public async Task<ActionResult<List<Employee>>> GetEmoloyeeById(Employee emp)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("INSERT INTO employee (name, age, address, salary) VALUES (@Name, @Age, @Address, @Salary);", emp);

            var employees = await connection.QueryAsync<Employee>("SELECT * from employee");

            return Ok(employees);
        }

        [HttpPut("/employee/{employeeId}")]
        public async Task<ActionResult<Employee>> UpdateEmployeeName(int employeeId, string name)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("UPDATE employee SET name = @name WHERE id = @id", new {name, id = employeeId } );

            var employee = await connection.QueryFirstAsync<Employee>("SELECT * from employee where id=@id",
                new { id = employeeId });

            return Ok(employee);
        }

        [HttpDelete("/employee/{employeeId}")]
        public async Task<ActionResult<List<Employee>>> DeleteEmployee(int employeeId)
        {
            var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("DELETE FROM employee WHERE id = @id", new { id = employeeId });

            var employees = await connection.QueryAsync<Employee>("SELECT * from employee");

            return Ok(employees);
        }
    }
}
