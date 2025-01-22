using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Helpers
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public string AppUserId { get; set; }
        public DateTime ExpiresOnUtc { get; set; }
        public AppUser User { get; set; }
    }
}
