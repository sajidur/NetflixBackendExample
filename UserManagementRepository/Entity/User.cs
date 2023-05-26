using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagementRepository.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
    }
    public enum SubscriptionType
    {
        Free,
        Basic,
        Premium
    }

}
