using System.ComponentModel.DataAnnotations;

namespace FaceLock.WebAPI.Models.UserModels.Request
{
    /// <summary>
    /// Request object for updating an account.
    /// </summary>
    public class UpdateAccountRequest
    {
        /// <summary>
        /// The username of the account being updated.
        /// </summary>
        /// <remarks>
        /// The username must be between 3 and 20 characters long.
        /// </remarks>
        /// <example>
        /// John.Doe
        /// </example>
        [Required(ErrorMessage = "Username is required")]
        [StringLength(101, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
        public string UserName { get; set; }

        /// <summary>
        /// The email address associated with the account being updated.
        /// </summary>
        /// <remarks>
        /// The email address must be in a valid format.
        /// </remarks>
        /// <example>
        /// john.doe@example.com
        /// </example>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        /// <summary>
        /// The first name associated with the account being updated.
        /// </summary>
        /// <remarks>
        /// The first name cannot exceed 50 characters.
        /// </remarks>
        /// <example>
        /// John
        /// </example>
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name associated with the account being updated.
        /// </summary>
        /// <remarks>
        /// The last name cannot exceed 50 characters.
        /// </remarks>
        /// <example>
        /// Doe
        /// </example>
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }
    }
}
