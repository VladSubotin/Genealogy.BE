using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class HierarchicTreeNodeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int X { get; set; }
        public int y { get; set; }
        public IEnumerable<HierarchicTreeNodeDTO> SubNodes { get; set; }
    }
}
