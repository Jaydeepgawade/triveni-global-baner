namespace EmployeeManagement.Application.DTOs;

public class SalaryDto
{
    public int Id { get; set; }
    public int EmpId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string? EmployeeName { get; set; }
}

public class CreateSalaryDto
{
    public int EmpId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class UpdateSalaryDto
{
    public int Id { get; set; }
    public int EmpId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class DepartmentMonthlySalaryDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public int Month { get; set; }
    public decimal TotalSalary { get; set; }
    public int Year { get; set; }
}
