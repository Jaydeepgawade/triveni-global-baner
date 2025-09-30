using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Domain.Entities;

public class Salary
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int EmpId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    [ForeignKey("EmpId")]
    public virtual Employee Employee { get; set; } = null!;
}
