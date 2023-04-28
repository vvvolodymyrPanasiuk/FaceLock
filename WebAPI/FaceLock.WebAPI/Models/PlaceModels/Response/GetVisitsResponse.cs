using System.Collections.Generic;
using System;

namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    /// <summary>
    /// Response object containing a list of a user's visits to places.
    /// </summary>
    public class GetVisitsResponse
    {
        /// <summary>
        /// List of a user's visits to places.
        /// </summary>
        /// <example>
        /// {
        ///     "visits": [
        ///         {
        ///             "id": 1,
        ///             "userId": "123456",
        ///             "placeId": 789,
        ///             "checkInTime": "2022-05-01T12:00:00Z",
        ///             "checkOutTime": "2022-05-01T14:00:00Z"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "userId": "123456",
        ///             "placeId": 890,
        ///             "checkInTime": "2022-05-02T09:00:00Z",
        ///             "checkOutTime": null
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<UserVisit> Visits { get; set; }

        public GetVisitsResponse(IEnumerable<UserVisit> visits)
        {
            Visits = visits;
        }
    }

    /// <summary>
    /// Object representing a user's visit to a place.
    /// </summary>
    public class UserVisit
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PlaceId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public UserVisit(
            int id,
            string userId,
            int placeId,
            DateTime checkInTime,
            DateTime? checkOutTime)
        {
            Id = id;
            UserId = userId;
            PlaceId = placeId;
            CheckInTime = checkInTime;
            CheckOutTime = checkOutTime;
        }
    }
}
