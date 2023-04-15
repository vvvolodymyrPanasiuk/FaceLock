namespace FaceLock.Authentication.DTO
{
    /// <summary>
    /// Data transfer object (DTO) for user login information.
    /// </summary>
    public class UserLoginDTO
    {
        /// <summary>
        /// Email of the user.
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password of the user.
        /// </summary>
        public string Password { get; set; }
    }
}
