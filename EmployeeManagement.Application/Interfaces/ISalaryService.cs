using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Interfaces;

public interface ISalaryService
{
    Task<SalaryDto?> GetByIdAsync(int id);
    Task<IEnumerable<SalaryDto>> GetAllAsync();
    Task<SalaryDto> CreateAsync(CreateSalaryDto createDto);
    Task<SalaryDto> UpdateAsync(UpdateSalaryDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<DepartmentMonthlySalaryDto>> GetDepartmentMonthlySalaryAsync(int year);
}
