using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LioProject.Domain.Entities.Helpers
{
    public interface ISoftDeletable
    {
        public int Id { get; set; }

        bool EstActif { get; set; }
        public int CreeParId { get; set; }

        public DateTime CreeLe { get; set; }
        public int ModifieEnDernierParId { get; set; }
        public DateTime ModifieEnDernierLe { get; set; }
    }
}
