using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Entities
{
 public    class useraccounts
    {
        public int ID { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string password_salt { get; set; }
        public Nullable<int> rolenum { get; set; }
    }
}
