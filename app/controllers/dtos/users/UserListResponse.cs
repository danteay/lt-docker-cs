using System.Collections.Generic;

namespace app.controllers.dtos.users
{
    /// <summary>
    /// Represents the response structure for a list of users with metadata.
    /// </summary>
    public class UserListResponse
    {
        /// <summary>
        /// The list of users.
        /// </summary>
        public List<UserResponse> Data { get; set; } = new List<UserResponse>();

        /// <summary>
        /// Metadata about the response.
        /// </summary>
        public Metadata Meta { get; set; } = new Metadata();
    }

    /// <summary>
    /// Represents metadata for the response.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// The total number of users.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of users per page.
        /// </summary>
        public int PageSize { get; set; }
    }
}
