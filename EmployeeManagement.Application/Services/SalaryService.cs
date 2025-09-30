using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SalaryService> _logger;

    public SalaryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SalaryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SalaryDto?> GetByIdAsync(int id)
    {
        try
        {
            var salary = await _unitOfWork.Salaries.GetByIdAsync(id);
            return salary != null ? _mapper.Map<SalaryDto>(salary) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting salary by ID: {SalaryId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<SalaryDto>> GetAllAsync()
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.GetAllAsync();
            return _mapper.Map<IEnumerable<SalaryDto>>(salaries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all salaries");
            throw;
        }
    }

    public async Task<SalaryDto> CreateAsync(CreateSalaryDto createDto)
    {
        try
        {
            var salary = _mapper.Map<Salary>(createDto);
            await _unitOfWork.Salaries.AddAsync(salary);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Salary created with ID: {SalaryId}", salary.Id);
            return _mapper.Map<SalaryDto>(salary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating salary");
            throw;
        }
    }

    public async Task<SalaryDto> UpdateAsync(UpdateSalaryDto updateDto)
    {
        try
        {
            var existingSalary = await _unitOfWork.Salaries.GetByIdAsync(updateDto.Id);
            if (existingSalary == null)
                throw new ArgumentException($"Salary with ID {updateDto.Id} not found");

            _mapper.Map(updateDto, existingSalary);
            await _unitOfWork.Salaries.UpdateAsync(existingSalary);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Salary updated with ID: {SalaryId}", updateDto.Id);
            return _mapper.Map<SalaryDto>(existingSalary);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating salary with ID: {SalaryId}", updateDto.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var salary = await _unitOfWork.Salaries.GetByIdAsync(id);
            if (salary == null)
                return false;

            await _unitOfWork.Salaries.DeleteAsync(salary);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Salary deleted with ID: {SalaryId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting salary with ID: {SalaryId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DepartmentMonthlySalaryDto>> GetDepartmentMonthlySalaryAsync(int year)
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.FindAsync(s => s.Date.Year == year);
            
            var result = salaries
                .GroupBy(s => new { s.Employee.Department.Name, s.Date.Month })
                .Select(g => new DepartmentMonthlySalaryDto
                {
                    DepartmentName = g.Key.Name,
                    Month = g.Key.Month,
                    Year = year,
                    TotalSalary = g.Sum(s => s.Amount)
                })
                .OrderBy(x => x.DepartmentName)
                .ThenBy(x => x.Month);

            _logger.LogInformation("Retrieved department monthly salary for year: {Year}", year);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting department monthly salary for year: {Year}", year);
            throw;
        }
    }
}
