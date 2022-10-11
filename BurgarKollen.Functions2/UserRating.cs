using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurgarKollen.Lib2
{
    public class UserRating
    {
        public int Id { get; set; }
        public int ResturantId { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string UserId { get; set; }

        public UserRating()
        {

        }
    }
}
