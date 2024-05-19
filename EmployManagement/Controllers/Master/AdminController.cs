/*using EmployManagement.Dto.Master;
using EmployManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployManagement.Controllers.Master
{
*//*    [Authorize(Roles ="Admin")]*//*
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _service;
        public AdminController(AdminService Service)
        {
            _service = Service;
        }
        *//*
             public IEnumerable<string>GetEmployees()
             {
                 return new List<string> { "Anas", "Ajish" };
             }*//*



        [HttpGet("GetEmployess")]
        public async Task<IActionResult> GetEmployees()
        {
            var result = await _service.GetEmployess();
            return Ok(result);
        }


        [HttpGet("GetEmployee/{id}")]
        public async Task<IActionResult> GetEmployees([FromRoute]int id)
        {
            var result = await _service.GeEmployeeById(id);
            return Ok(result);
        }


        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto model)
        {
            var result = await _service.UpdateEmployee(model);
            return Ok(result);
        }



        [HttpPut("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var result = await _service.DeleteEmployee(id);
            return Ok(result);
        }




    }

}
*/