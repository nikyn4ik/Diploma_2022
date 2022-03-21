using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Storage
    {
        public int id_storage { get; set; }
        public string name_storage { get; set; }
        public string address { get; set; }
        public string phone_storage { get; set; }  
        public DateTime date_of_entrance { get; set; }
        public string SAP_product_code { get; set; }
        public string remainder { get; set; }
    }
}
