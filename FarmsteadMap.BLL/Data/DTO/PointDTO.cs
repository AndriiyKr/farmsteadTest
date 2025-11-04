using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmsteadMap.BLL.Data.DTO
{
    public class PointDTO
    {
        public double X { get; set; }
        public double Y { get; set; }
        public PointDTO(double x, double y) { X = x; Y = y; }
    }
}
