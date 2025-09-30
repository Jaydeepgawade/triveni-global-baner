using AutoMapper;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            return employee != null ? _mapper.Map<EmployeeDto>(employee) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employee by ID: {EmployeeId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        try
        {
            var employees = await _unitOfWork.Employees.GetAllAsync();
            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all employees");
            throw;
        }
    }

    public async Task<PagedResult<EmployeeDto>> GetPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            var totalCount = await _unitOfWork.Employees.CountAsync();
            var employees = await _unitOfWork.Employees.FindAsync(e => true);
            var pagedEmployees = employees.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            
            return new PagedResult<EmployeeDto>
            {
                Items = _mapper.Map<List<EmployeeDto>>(pagedEmployees),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paged employees");
            throw;
        }
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createDto)
    {
        try
        {
            var employee = _mapper.Map<Employee>(createDto);
            await _unitOfWork.Employees.AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Employee created with ID: {EmployeeId}", employee.Id);
            return _mapper.Map<EmployeeDto>(employee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            throw;
        }
    }

    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto updateDto)
    {
        try
        {
            var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(updateDto.Id);
            if (existingEmployee == null)
                throw new ArgumentException($"Employee with ID {updateDto.Id} not found");

            _mapper.Map(updateDto, existingEmployee);
            await _unitOfWork.Employees.UpdateAsync(existingEmployee);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Employee updated with ID: {EmployeeId}", updateDto.Id);
            return _mapper.Map<EmployeeDto>(existingEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee with ID: {EmployeeId}", updateDto.Id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            if (employee == null)
                return false;

            await _unitOfWork.Employees.DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("Employee deleted with ID: {EmployeeId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee with ID: {EmployeeId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeSearchDto>> SearchByNameAsync(string name)
    {
        try
        {
            var employees = await _unitOfWork.Employees.FindAsync(e => 
                e.Name.ToLower().Contains(name.ToLower()));
            
            return _mapper.Map<IEnumerable<EmployeeSearchDto>>(employees);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching employees by name: {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeSalaryRangeDto>> GetEmployeesBySalaryRangeAsync(decimal minSalary, decimal maxSalary)
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.FindAsync(s => 
                s.Amount >= minSalary && s.Amount <= maxSalary);
            
            return salaries.Select(s => new EmployeeSalaryRangeDto
            {
                Id = s.Employee.Id,
                Name = s.Employee.Name,
                Salary = s.Amount,
                DepartmentName = s.Employee.Department.Name
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employees by salary range: {MinSalary} - {MaxSalary}", minSalary, maxSalary);
            throw;
        }
    }

    public async Task<string?> UploadImageAsync(int employeeId, IFormFile imageFile)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null)
                throw new ArgumentException($"Employee with ID {employeeId} not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "employees");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{employeeId}_{DateTime.Now:yyyyMMddHHmmss}_{Path.GetFileName(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            employee.ImagePath = $"/images/employees/{fileName}";
            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Image uploaded for employee ID: {EmployeeId}", employeeId);
            return employee.ImagePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image for employee ID: {EmployeeId}", employeeId);
            throw;
        }
    }

    public async Task<bool> DeleteImageAsync(int employeeId)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
            if (employee == null || string.IsNullOrEmpty(employee.ImagePath))
                return false;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", employee.ImagePath.TrimStart('/'));
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            employee.ImagePath = null;
            await _unitOfWork.Employees.UpdateAsync(employee);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Image deleted for employee ID: {EmployeeId}", employeeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image for employee ID: {EmployeeId}", employeeId);
            throw;
        }
    }
}
