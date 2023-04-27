namespace FaceLock.WebAPI.Models.DoorLockModels.Response
{
    public class GetDoorLockResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public GetDoorLockResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
