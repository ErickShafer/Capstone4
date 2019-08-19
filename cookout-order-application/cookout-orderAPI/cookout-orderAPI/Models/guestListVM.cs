using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookout_orderAPI.Models
{
    public class GuestListVM
    {

        public List<int> guestIds { get; set; }

        public int eventId { get; set; }
    }
}
