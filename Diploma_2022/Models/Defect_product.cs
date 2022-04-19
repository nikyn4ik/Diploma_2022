using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    internal class Defect_product
    {
        public int defect_product { get; set; }
        public int id_order { get; set; }
        public string reasons_for_sending { get; set; }
        public DateTime product_for_sending { get; set; }
    }
}