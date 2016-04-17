using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refs
{
    public interface ICell
    {
        int Row { get; set; }
        int Column { get; set; }
        int? Value { get; set; }
    }
}
