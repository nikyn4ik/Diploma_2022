using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Type_product
    {
        public int id_type_product { get; set; }
        public int id_aggregate { get; set; }
        public int id_order { get; set; }
        public string product_name { get; set; }
        public string unit { get; set; }
        public string product_description { get; set; }
        public string SAP_product_code { get; set; }
    }
}
