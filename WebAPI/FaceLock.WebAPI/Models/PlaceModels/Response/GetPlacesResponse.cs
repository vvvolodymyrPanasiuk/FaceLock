using System.Collections.Generic;

namespace FaceLock.WebAPI.Models.PlaceModels.Response
{
    /// <summary>
    /// Response object for getting list of places.
    /// </summary>
    public class GetPlacesResponse
    {
        /// <summary>
        /// List of places.
        /// </summary>
        /// <remarks>
        /// The list of places returned by the API.
        /// </remarks>
        /// <example>
        /// Sample response:
        /// {
        ///     "places": [
        ///         {
        ///             "id": 1,
        ///             "name": "Place1",
        ///             "description": "Place1 description"
        ///         },
        ///         {
        ///             "id": 2,
        ///             "name": "Place2",
        ///             "description": "Place2 description"
        ///         }
        ///     ]
        /// }
        /// </example>
        public IEnumerable<GetPlaceResponse> Places { get; set; }

        public GetPlacesResponse(IEnumerable<GetPlaceResponse> places) 
        {
            Places = places;
        }
    }
}
