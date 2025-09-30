using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Domain.Entities;

public class Employee
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Phone]
    [MaxLength(20)]
    public string Phone { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(10)]
    public string Gender { get; set; } = string.Empty;
    
    [Required]
    public DateTime DOB { get; set; }
    
    [Required]
    public int DeptId { get; set; }
    
    [MaxLength(500)]
    public string? ImagePath { get; set; }
    
    [ForeignKey("DeptId")]
    public virtual Department Department { get; set; } = null!;
    
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
