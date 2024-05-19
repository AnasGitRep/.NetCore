using AutoMapper;
using EmployManagement.Data;
using EmployManagement.Dto.Base;
using EmployManagement.Dto.Master;
using EmployManagement.Models.Master;
using Microsoft.EntityFrameworkCore;

namespace EmployManagement.Services
{
    public class AdminService
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public AdminService
            (
            ApplicationDbContext DataContext,
            IMapper Mapper
            )
        {
            _context = DataContext;
            _mapper = Mapper;
        }


        public async Task<ResponseModel<EmployeeDto>> GetEmployess()
        {
            ResponseModel<EmployeeDto> response = new ResponseModel<EmployeeDto>();

            try
            {
                var res = await _context.Employees.ToListAsync();
                if (res != null)
                {
                    var employees = _mapper.Map<List<EmployeeDto>>(res);
                    response.Items = employees;
                    response.IsOk = true;
                }
                else
                {
                    response.Message = "No employees found";
                    response.IsOk &= false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<ResponseModel<EmployeeDto>> GeEmployeeById(int id)
        {
            ResponseModel<EmployeeDto> response = new ResponseModel<EmployeeDto>();
            try
            {
                if (id > 0)
                {
                    var res = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

                    if (res != null)
                    {
                        var employee = _mapper.Map<EmployeeDto>(res);
                        response.Item = employee;
                        response.IsOk = true;
                    }
                    else
                    {
                        response.Message = "No employee found";
                        response.IsOk &= false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<ResponseModel<EmployeeDto>> UpdateEmployee(EmployeeDto model)
        {
            ResponseModel<EmployeeDto> response = new ResponseModel<EmployeeDto>();

            try
            {
                var employee = await _context.Employees.FindAsync(model.Id);
                if (employee == null)
                {
                    response.IsOk = false;
                    response.Message = "Employee not found";
                    return response;
                }
                else
                {
                    _mapper.Map(model, employee);
                    await _context.SaveChangesAsync();
                    response.IsOk = true;
                    response.Message = "Employee updated successfully";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        public async Task<ResponseModel<EmployeeDto>> DeleteEmployee(int id)
        {
            ResponseModel<EmployeeDto> response = new ResponseModel<EmployeeDto>();

            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null)
                {
                    response.IsOk = false;
                    response.Message = "Employee not found";
                    return response;
                }
                else
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                    response.IsOk = true;
                    response.Message = "Employee delete successfully";

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
