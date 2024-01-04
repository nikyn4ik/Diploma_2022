using System;

namespace Diploma_2022.Models
{
    class Orders
    {
        public int id_order { get; set; }
        public string syst_c3 { get; set; }
        public string log_c3 { get; set; }
        public DateTime date_of_delivery { get; set; }
        public DateTime date_of_entrance { get; set; }
        public DateTime date_of_adoption { get; set; }
        public double thickness_mm { get; set; }
        public double width_mm { get; set; }
        public double length_mm { get; set; }
        public string name_product { get; set; }
        public string name_consignee { get; set; }
        public string status_order { get; set; }
        public int id_payer { get; set; }
        public int id_consignee { get; set; }
        public string mark { get; set; }
        public int id_qua_certificate { get; set; }
        public string access_standart { get; set; }
    }
}
