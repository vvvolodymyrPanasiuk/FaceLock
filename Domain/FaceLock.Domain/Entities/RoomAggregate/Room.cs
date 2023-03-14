using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.Domain.Entities.RoomAggregate
{
    /// <summary>
    /// Kлас, що представляє кімнату
    /// </summary>
    public class Room
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NumberRoom { get; set; }

        public List<Visit>? Visits { get; set; }
    }
}
