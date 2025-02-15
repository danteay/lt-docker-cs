using app.controllers.dtos.users;
using app.domains.users.service;
using app.infra;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace app.controllers
{
    /// <summary>
    /// Controller for managing users.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns>A list of users wrapped in a "data" object.</returns>
        [HttpGet]
        public async Task<ActionResult<UserListResponse>> GetAll(int page = 1, int pageSize = 10)
        {
            var users = await userService.GetAllAsync();
            var userResponses = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                }).ToList();

            var response = new UserListResponse
            {
                Data = userResponses,
                Meta = new Metadata
                {
                    Total = users.Count,
                    Page = page,
                    PageSize = pageSize
                }
            };

            return Ok();
        }

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(int id)
        {
            var user = await userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return Ok(userResponse);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request">The user to create.</param>
        /// <returns>The created user.</returns>
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] UserRequest request)
        {
            var user = new app.domains.users.User
            {
                Name = request.Name,
                Email = request.Email
            };

            var createdUser = await userService.CreateAsync(user);

            var userResponse = new UserResponse
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email
            };

            return CreatedAtAction(nameof(GetById), new { id = userResponse.Id }, userResponse);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="request">The updated user data.</param>
        /// <returns>The updated user.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(int id, [FromBody] UserRequest request)
        {
            var user = new app.domains.users.User
            {
                Name = request.Name,
                Email = request.Email
            };

            var updatedUser = await userService.UpdateAsync(id, user);
            if (updatedUser == null)
                return NotFound();

            var userResponse = new UserResponse
            {
                Id = updatedUser.Id,
                Name = updatedUser.Name,
                Email = updatedUser.Email
            };

            return Ok(userResponse);
        }

        /// <summary>
        /// Deletes a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content if the user was deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await userService.DeleteAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpGet("hello")]
        public string Hello()
        {
            return $"Hello, World! {Configuration.GetInstance.Get("NAME_USER")}";
        }
    }
}