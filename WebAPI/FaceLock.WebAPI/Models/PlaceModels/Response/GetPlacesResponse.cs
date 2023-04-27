using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    public class GetPlacesResponse
    {
        public IEnumerable<GetPlaceResponse> Places { get; set; }

        public GetPlacesResponse(IEnumerable<GetPlaceResponse> places) 
        {
            Places = places;
        }
    }
}
