using Findash.Common.DTOs;
using Findash.Users.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Findash.Users;

public class UsersController(
    IUserRepository userRepository,
    ILogger<UsersController> logger)
    : BaseController
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UsersController> _logger = logger;
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetUsersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRequest? request)
    {
        var users = await _userRepository.GetAll(request?.Page, request?.RecordsPerPage, request?.Contains);
        
        return Ok(users.Select(UserToGetUserResponse));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(IEnumerable<GetUsersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid user identifier format");
        }

        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            return NotFound();
        }
    
        var response = UserToGetUserResponse(user);
        return Ok(response);
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var newUser = new User
        {
            Id = new Guid(),
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            UserName = request.UserName!,
            Email = request.Email!,
            BirthDate = (DateOnly)request.BirthDate!,
            Gender = (Gender)request.Gender!,
        };

        try
        {
            await _userRepository.Create(newUser);
            
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a new user: \n{Message}", ex.Message);
            return StatusCode(500, "An error occurred while creating a new user");
        }
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(GetUsersResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var existingUser = await _userRepository.GetById(id);
        if (existingUser == null)
        {
            _logger.LogWarning("User with Id: {UserId} not found", id);
            return NotFound();
        }
        
        try
        {
            existingUser.FirstName = string.IsNullOrWhiteSpace(request.FirstName) ? existingUser.FirstName : request.FirstName;
            existingUser.LastName = string.IsNullOrWhiteSpace(request.LastName) ? existingUser.LastName : request.LastName;
            existingUser.BirthDate = request.BirthDate ?? existingUser.BirthDate;
            existingUser.Gender = request.Gender ?? existingUser.Gender;
            
            await _userRepository.Update(existingUser);
            var response = UserToGetUserResponse(existingUser);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with Id: {UserId}\n{Message}", id, ex.Message);
            return StatusCode(500, "An error occurred while updating the user");
        }
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            await _userRepository.Delete(user);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with Id: {UserId}\n{Message}", id, ex.Message);
            return StatusCode(500, "An error occurred while deleting the user");
        }
    }
    
    private static GetUsersResponse UserToGetUserResponse(User user)
    {
        return new GetUsersResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Gender = user.Gender
        };
    }
    
    //TODO Accounts
    
    // [HttpGet("{employeeId}/benefits")]
    // [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefit>), StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status404NotFound)]
    // [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    // public async Task<IActionResult> GetBenefitsForEmployee(int employeeId)
    // {
    //     // var employee = await _dbContext.Employees
    //     //     .Include(e => e.Benefits)
    //     //     .ThenInclude(e => e.Benefit)
    //     //    // .SingleOrDefaultAsync(e => e.Id == employeeId);
    //     Employee employee = null!;
    //     await Task.CompletedTask;
    //     if (employee == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     var benefits = employee.Benefits.Select(b => new GetEmployeeResponseEmployeeBenefit
    //     {
    //         Id = b.Id,
    //         Name = b.Benefit.Name,
    //         Description = b.Benefit.Description,
    //         Cost = b.CostToEmployee ?? b.Benefit.BaseCost   //we want to use the cost to employee if it exists, otherwise we want to use the base cost
    //     });
    //
    //     return Ok(benefits);
    // }
    
}