using FaceLock.Domain.Entities.PlaceAggregate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceLock.Domain.Entities.UserAggregate
{
    // ApplicationUser - це клас користувача, який успадковує клас IdentityUser з пакету Microsoft.AspNetCore.Identity
    // IdentityUser містить ряд полів, які ми можемо використовувати для автентифікації та авторизації користувача

    /// <summary>
    /// User
    /// </summary>
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Status { get; set; }


        // UserFace - це поле, в якому ми зберігаємо фото користувача у вигляді масиву байтів
        public List<UserFace>? UserFaces { get; set; }  
        public List<Visit>? Visits { get; set; }
    }
}
