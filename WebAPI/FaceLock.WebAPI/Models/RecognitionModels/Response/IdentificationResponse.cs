namespace FaceLock.WebAPI.Models.RecognitionModels.Response
{
    /// <summary>
    /// Identification response object.
    /// </summary>
    public class IdentificationResponse
    {
		/// <summary>
		/// The ID of the user.
		/// </summary>
		/// <example>12345</example>
		public string Id { get; set; }

		/// <summary>
		/// The username of the user.
		/// </summary>
		/// <example>johndoe</example>
		public string Username { get; set; }

		/// <summary>
		/// The email address of the user.
		/// </summary>
		/// <example>johndoe@example.com</example>
		public string Email { get; set; }

		/// <summary>
		/// The first name of the user.
		/// </summary>
		/// <example>John</example>
		public string FirstName { get; set; }

		/// <summary>
		/// The last name of the user.
		/// </summary>
		/// <example>Doe</example>
		public string LastName { get; set; }

		/// <summary>
		/// The status of the user.
		/// </summary>
		/// <example>User</example>
		public string Status { get; set; }
		/// <summary>
		/// The prediction distance by recognition.
		/// </summary>
		/// <remarks>
		/// A measure of similarity between the recognized face and the reference face.
		/// A smaller value indicates higher similarity, and a larger value indicates lower similarity.
		/// </remarks>
		/// <example>
		/// "29.41"
		/// </example>
		public double? PredictionDistance { get; set; }

		public IdentificationResponse(
		 string id,
		 string username,
		 string email,
		 string firstname,
		 string lastname,
		 string status,
		 double? predictionDistance)
		{
			Id = id;
			Username = username;
			Email = email;
			FirstName = firstname;
			LastName = lastname;
			Status = status;
			PredictionDistance = predictionDistance;
		}
	}
}