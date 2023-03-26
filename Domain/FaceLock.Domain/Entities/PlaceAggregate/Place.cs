namespace FaceLock.Domain.Entities.PlaceAggregate
{
    /// <summary>
    /// Class representing the place of inspection
    /// </summary>
    public class Place
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<Visit>? Visits { get; set; }
    }
}
