using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Domain.Entities;

public class Department
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
