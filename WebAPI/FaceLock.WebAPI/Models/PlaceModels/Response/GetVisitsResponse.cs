using System.Collections.Generic;
using System;

namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    public class GetVisitsResponse
    {
        public IEnumerable<UserVisit> Visits { get; set; }

        public GetVisitsResponse(IEnumerable<UserVisit> visits)
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
