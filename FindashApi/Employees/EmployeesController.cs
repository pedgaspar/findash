﻿using Findash.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Findash.Employees;

public class EmployeesController : BaseController
{
    private readonly IRepository<Employee> _repository;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IRepository<Employee> repository, 
        ILogger<EmployeesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult GetAllEmployees()
    {
        var employees = _repository.GetAll().Select(EmployeeToGetEmployeeResponse);

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetEmployeeById(int id)
    {
        var employee = _repository.GetById(id);
        if (employee == null)
        {
            return NotFound();
        }

        var employeeResponse = EmployeeToGetEmployeeResponse(employee);
        return Ok(employeeResponse);
    }

    [HttpPost]
    public IActionResult CreateEmployee(CreateEmployeeRequest employeeRequest)
    {

        var newEmployee = new Employee {
            FirstName = employeeRequest.FirstName!,
            LastName = employeeRequest.LastName!,
            SocialSecurityNumber = employeeRequest.SocialSecurityNumber,
            Address1 = employeeRequest.Address1,
            Address2 = employeeRequest.Address2,
            City = employeeRequest.City,
            State = employeeRequest.State,
            ZipCode = employeeRequest.ZipCode,
            PhoneNumber = employeeRequest.PhoneNumber,
            Email = employeeRequest.Email
        };
        _repository.Create(newEmployee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] UpdateEmployeeRequest employeeRequest)
    {
        _logger.LogInformation("Updating employee with id: {id}", id);
        var existingEmployee = _repository.GetById(id);
        if (existingEmployee == null)
        {
            _logger.LogWarning("Employee with id: {id} does not exist", id);
            return NotFound();
        }

        existingEmployee.Address1 = employeeRequest.Address1;
        existingEmployee.Address2 = employeeRequest.Address2;
        existingEmployee.City = employeeRequest.City;
        existingEmployee.State = employeeRequest.State;
        existingEmployee.ZipCode = employeeRequest.ZipCode;
        existingEmployee.PhoneNumber = employeeRequest.PhoneNumber;
        existingEmployee.Email = employeeRequest.Email;

        _repository.Update(existingEmployee);
        return Ok(existingEmployee);
    }
    
    private static GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee)
    {
        return new GetEmployeeResponse
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Address1 = employee.Address1,
            Address2 = employee.Address2,
            City = employee.City,
            State = employee.State,
            ZipCode = employee.ZipCode,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            Benefits = employee.Benefits.Select(BenefitToBenefitResponse).ToList()
        };
    }

    private static GetEmployeeResponseEmployeeBenefit BenefitToBenefitResponse(EmployeeBenefits benefit)
    {
        return new GetEmployeeResponseEmployeeBenefit
        {
            Id = benefit.Id,
            EmployeeId = benefit.EmployeeId,
            BenefitType = benefit.BenefitType,
            Cost = benefit.Cost
        };
    }
    
    [HttpGet("{employeeId}/benefits")]
    [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetBenefitsForEmployee(int employeeId)
    {
        var employee = _repository.GetById(employeeId);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee.Benefits.Select(BenefitToBenefitResponse));
    }
}