namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    /// <summary>
    /// Response object for getting a place by ID.
    /// </summary>
    public class GetPlaceResponse
    {
        /// <summary>
        /// The unique identifier of the place.
        /// </summary>
        /// <remarks>
        /// This ID can be used to identify the place in other API calls.
        /// </remarks>
        /// <example>
        /// 42
        /// </example>
        public int Id { get; set; }

        /// <summary>
        /// The name of the place.
        /// </summary>
        /// <remarks>
        /// The name of the place as it is displayed to users.
        /// </remarks>
        /// <example>
        /// Central Park
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// The description of the place.
        /// </summary>
        /// <remarks>
        /// A brief description of the place.
        /// </remarks>
        /// <example>
        /// Central Park is an urban park in New York City, located between the Upper West Side and the Upper East Side.
        /// </example>
        public string Description { get; set; }

        public GetPlaceResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
