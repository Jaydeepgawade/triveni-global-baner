using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            return department != null ? _mapper.Map<DepartmentDto>(department) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting department by ID: {DepartmentId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all departments");
            throw;
        }
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto createDto)
    {
        try
        {
            var department = _mapper.Map<Department>(createDto);
            await _unitOfWork.Departments.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Department created with ID: {DepartmentId}", department.Id);
            return _mapper.Map<DepartmentDto>(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            throw;
        }
    }

    public async Task<DepartmentDto> UpdateAsync(UpdateDepartmentDto updateDto)
    {
        try
        {
            var existingDepartment = await _unitOfWork.Departments.GetByIdAsync(updateDto.Id);
            if (existingDepartment == null)
                throw new ArgumentException($"Department with ID {updateDto.Id} not found");

            _mapper.Map(updateDto, existingDepartment);
            await _unitOfWork.Departments.UpdateAsync(existingDepartment);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Department updated with ID: {DepartmentId}", updateDto.Id);
            return _mapper.Map<DepartmentDto>(existingDepartment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department with ID: {DepartmentId}", updateDto.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id);
            if (department == null)
                return false;

            // Check if department has employees
            var hasEmployees = await _unitOfWork.Employees.ExistsAsync(e => e.DeptId == id);
            if (hasEmployees)
                throw new InvalidOperationException("Cannot delete department that has employees");

            await _unitOfWork.Departments.DeleteAsync(department);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Department deleted with ID: {DepartmentId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department with ID: {DepartmentId}", id);
            throw;
        }
    }
}
