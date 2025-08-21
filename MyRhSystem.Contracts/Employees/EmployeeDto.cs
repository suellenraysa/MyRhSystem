using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRhSystem.Contracts.Employees
{
    public record EmployeeDto(
    int Id,
    string Nome,
    string Email,
    string Departamento,
    string Cargo,
    bool Ativo);

}
