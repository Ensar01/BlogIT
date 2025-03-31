using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogIT.Model.DataTransferObjects
{
    public record UserTokenDto(string Email, string UserName, string Id);
}
