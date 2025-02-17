namespace app.controllers.dtos.users
{
    /// <summary>
    /// Represents the response data for a user.
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// The ID of the user.
        /// </summary>
        public int Id { get; set; }

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
