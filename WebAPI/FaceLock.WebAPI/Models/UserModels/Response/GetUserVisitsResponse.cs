using System;
using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.UserModels.Response
{
    public class GetUserVisitsResponse
    {
        public IEnumerable<UserVisit> Visits { get; set; }

        public GetUserVisitsResponse(IEnumerable<UserVisit> visits)
        {
            Visits = visits;
        }
    }

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
