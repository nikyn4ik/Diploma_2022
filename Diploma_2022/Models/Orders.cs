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
        public string name_product { get; set; }
        public string status_order { get; set; }
        public string consignee { get; set; }
        public int id_payer { get; set; }
        public int id_consignee { get; set; }
        public string mark { get; set; }
        public string standart_mark { get; set; }
        public int id_storage { get; set; }
        public int id_qua_certificate { get; set; }
        public string access_standart { get; set; }
    }
}
