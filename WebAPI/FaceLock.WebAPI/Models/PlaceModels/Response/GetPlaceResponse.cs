namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    public class GetPlaceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public GetPlaceResponse(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
