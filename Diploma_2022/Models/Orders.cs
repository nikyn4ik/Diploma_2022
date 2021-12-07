using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Orders
    {
        public int syst_c3 { get; set; }
        public int log_c3 { get; set; }
        public int position_c3 { get; set; }
        public int number_of_changes { get; set; }
        public string error_at_checkout { get; set; }
        public string consignee { get; set; }
        public int stan { get; set; }
        public int order_quantity_tons { get; set; }
        public string prohibitions { get; set; }
        public DateTime date_of_delivery { get; set; }//
        public DateTime date_of_entrance { get; set; }
        public DateTime date_of_adoption { get; set; }
        public DateTime id_type_product { get; set; }
        public string incoterms { get; set; }
        public string standard_per_mark { get; set; }
        public string mark { get; set; }
        public string additional_mark { get; set; }
        public float thickness_mm { get; set; }
        public float thickness_max_mm { get; set; }
        public float width_mm { get; set; }
        public float width_max_mm { get; set; }
        public float length_mm { get; set; }
        public float length_max_mm { get; set; }
        public int tolerance_standart { get; set; }
        public string product_standard { get; set; }
        public string route_map { get; set; }
        public string order_status { get; set; }
        public DateTime date_of_change { get; set; }
        public string order_type { get; set; }
        public string internal_order_type { get; set; }
        public string payer { get; set; }
        public int shipment_total_amount_tons { get; set; }
         public int remainder { get; set; }
         public string SAP_product_code { get; set; } 
         public int notes_planner_to_order { get; set; }
          public int notes_planner_a_to_position { get; set; }
          public DateTime planning_date_of_lifting_the_ban { get; set; }//
         public float planning_dispensed_volume_tons { get; set; }
           public string direction { get; set; }
          public float number_of_orders_per_month_tons { get; set; }
         public float  number_of_shipments_per_month_tons { get; set; }
          public float remaining_monthly_tons { get; set; }
          public float  total_availability_tons { get; set; }
          public string early_shipment { get; set; }
          public string changes_in_letters { get; set; }
        public float  remaining_weight_APO_tons { get; set; }
         public string sorting_type { get; set; }
        public int  order_status_indicator { get; set; }
    }
}
