using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MyRhSystem.Domain.Entities.Benefits;

namespace MyRhSystem.Domain.Entities.Employees
{
    [Table("employee_benefits")]
    [PrimaryKey(nameof(EmployeeId), nameof(BenefitTypeId))] 
    public class EmployeeBenefits
    {
        [ForeignKey(nameof(Employee))]
        [Column("employee_id")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [ForeignKey(nameof(BenefitType))]
        [Column("benefit_type_id")]
        public int BenefitTypeId { get; set; }
        public BenefitTypes BenefitType { get; set; } = default!;
    }
}
