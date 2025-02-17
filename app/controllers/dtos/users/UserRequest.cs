namespace app.controllers.dtos.users
{
    /// <summary>
    /// Represents the request data for creating or updating a user.
    /// </summary>
    public class UserRequest
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;
    }
}
