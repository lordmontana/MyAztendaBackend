using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities
{
    /// <summary>
    /// All tenant tables inherit these common columns.
    /// </summary>
    public abstract class BaseEntity : IInstallation
    {
        public int Id { get; set; }                    
        public int IId {get; set; }        
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedUtc { get; set; }
    }
}
