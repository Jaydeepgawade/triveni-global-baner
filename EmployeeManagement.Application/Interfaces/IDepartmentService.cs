using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto);
    Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto updateDto);
    Task<bool> DeleteAsync(int id);
}
