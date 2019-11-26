using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Model
{
    public class User
    {
        public System.Guid UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public ICollection<Choice> Choices { get; set; }
    }

}
