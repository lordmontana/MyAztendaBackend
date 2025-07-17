using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{   
     /// <summary>
     /// Implemented by every entity that belongs to one tenant/installation.
     /// </summary>
    public interface IInstallation
    {
        int IId { get; set; }   // maps to column i_id
    }
}
