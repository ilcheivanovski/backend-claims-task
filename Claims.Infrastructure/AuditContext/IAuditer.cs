using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claims.Infrastructure.AuditContext
{
    public interface IAuditer
    {
        Task AuditClaim(string id, string httpRequestType);
        Task AuditCover(string id, string httpRequestType);
    }
}
