using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RefreshTokenDto
    {
        public required string RefreshToken { get; init; }
    }
}
