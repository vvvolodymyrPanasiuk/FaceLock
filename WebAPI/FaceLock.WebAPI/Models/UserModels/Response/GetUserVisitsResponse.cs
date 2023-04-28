using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    /// <summary>
    /// Response object for getting user visits.
    /// </summary>
    public class GetUserVisitsResponse
    {
        /// <summary>
        /// List of user visits.
        /// </summary>
        /// <remarks>
        /// A collection of user visits that includes details about the place, check-in time and check-out time (if available).
        /// </remarks>
        /// <example>
        /// {
        ///     "visits": [
        ///         {
        ///             "id": 1,
        ///             "userId": "123456",
        ///             "placeId": 789,
        ///             "checkInTime": "2023-04-28T10:00:00",
        ///             "checkOutTime": "2023-04-28T18:00:00"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "userId": "123456",
        ///             "placeId": 123,
        ///             "checkInTime": "2023-04-27T14:00:00",
        ///             "checkOutTime": null
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<UserVisitByUserResponse> Visits { get; set; }

        public GetUserVisitsResponse(IEnumerable<UserVisitByUserResponse> visits)
        {
            Visits = visits;
        }
    }

    /// <summary>
    /// Object representing a user visit.
    /// </summary>
    public class UserVisitByUserResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PlaceId { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public UserVisitByUserResponse(
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
