using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Orders
    {
        public int id_order { get; set; }
        public int syst_c3 { get; set; }
        public int log_c3 { get; set; }
        public DateTime date_of_delivery { get; set; }
        public DateTime date_of_entrance { get; set; }
        public DateTime date_of_adoption { get; set; }
        public float thickness_mm { get; set; }
        public float width_mm { get; set; }
        public float length_mm { get; set; }
        public float thickness_max_mm { get; set; }
        public float width_max_mm { get; set; }
        public float length_max_mm { get; set; }
        public string SAP_product_code { get; set; } 
    }
}
