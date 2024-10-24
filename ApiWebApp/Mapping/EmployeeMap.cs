using ApiWebApp.Dto.CustomerDto;
using DAL.Models.ItemsModel;
using Microsoft.Identity.Client;

namespace ApiWebApp.Mapping;

public static class EmployeeMap
{
    public static  Employee ToEntity(this EmployeeDto employeeDto)
    {
        if (employeeDto is null)
        {
            throw new ArgumentNullException(nameof(employeeDto));
        }

        return new Employee
        {
            EmployeeId = employeeDto.EmployeeId,
            FullName = employeeDto.FullName,
            Email = employeeDto.Email,
            Phone = employeeDto.Phone,
            IsActive = employeeDto.IsActive,
            JobTitle = employeeDto.JobTitle,
            CustomerId = employeeDto.CustomerId
        };

    }
    public static void  UpdateEntity(this EmployeeDto employeeDto, Employee employee)
    {
        if (employeeDto  is null)
        {
            throw new ArgumentNullException(nameof(employeeDto));
        }

        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        employee.FullName = employeeDto.FullName;
        employee.Email = employeeDto.Email;
        employee.Phone = employeeDto.Phone;
        employee.IsActive = employeeDto.IsActive;
        employee.JobTitle = employeeDto.JobTitle;
        employee.CustomerId = employeeDto.CustomerId;
    } 

    public static EmployeeDto ToDto(this Employee employee)
    {
        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            FullName = employee.FullName,
            Email = employee.Email,
            Phone = employee.Phone,
            IsActive = employee.IsActive,
            JobTitle = employee.JobTitle,
            CustomerId = employee.CustomerId
        };
    }
}
