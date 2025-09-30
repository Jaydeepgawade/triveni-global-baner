using EmployeeManagement.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EmployeeSearchDto>> SearchByNameAsync(string name);
    Task<IEnumerable<EmployeeSalaryRangeDto>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary);
    Task<string?> UploadImageAsync(int employeeId, IFormFile imageFile);
    Task<bool> DeleteImageAsync(int employeeId);
}
