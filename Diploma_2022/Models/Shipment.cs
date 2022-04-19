using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Shipment
    {
        public int id_shipment { get; set; }
        public string early_delivery { get; set; }
        public DateTime date_of_shipments { get; set; }
        public int shipment_total_amount_tons { get; set; }
        public float number_of_shipments_per_month_tons { get; set; }
        public int id_storage { get; set; }
        public int id_order { get; set; }

    }
}
