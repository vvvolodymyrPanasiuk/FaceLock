﻿using FaceLock.Domain.Entities.PlaceAggregate;


namespace FaceLock.Domain.Repositories.PlaceRepository
{
    /// <summary>
    /// Interface inherited from IRepository for working with the Places table in the database
    /// </summary>
    public interface IPlaceRepository : IRepository<Place>
    {
    }
}
