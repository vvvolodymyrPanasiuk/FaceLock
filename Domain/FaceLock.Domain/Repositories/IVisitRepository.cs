using FaceLock.Domain.Entities.PlaceAggregate;


namespace FaceLock.Domain.Repositories
{
    public interface IVisitRepository : IRepository<Visit>
    {
        Task<List<Visit>> GetVisitsByUserIdAsync(string userId);
        Task<List<Visit>> GetVisitsByPlaceIdAsync(int roomId);
    }
}
