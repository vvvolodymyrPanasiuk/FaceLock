namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    /// <summary>
    /// Response object for getting a single door lock.
    /// </summary>
    public class GetDoorLockResponse
    {
        /// <summary>
        /// The ID of the door lock.
        /// </summary>
        /// <remarks>
        /// This is a unique identifier for the door lock.
        /// </remarks>
        /// <example>
        /// 1
        /// </example>
        public int Id { get; set; }

        /// <summary>
        /// The name of the door lock.
        /// </summary>
        /// <remarks>
        /// This is the name of the door lock.
        /// </remarks>
        /// <example>
        /// Front Door
        /// </example>
        public string Name { get; set; }

        /// <summary>
        /// The description of the door lock.
        /// </summary>
        /// <remarks>
        /// This is the description of the door lock.
        /// </remarks>
        /// <example>
        /// This door lock is located at the front entrance of the building.
        /// </example>
        public string Description { get; set; }

        public GetDoorLockResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
