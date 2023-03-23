﻿namespace FaceLock.Domain.Entities.PlaceAggregate
{
    /// <summary>
    /// Kлас, що представляє місце огляду
    /// </summary>
    public class Place
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<Visit>? Visits { get; set; }
    }
}