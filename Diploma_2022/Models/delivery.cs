using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class delivery
    {
        public int id_delivery { get; set; }
        public string consignee { get; set; }
        public string product_standard { get; set; }
        public string name_storage { get; set; }
        public DateTime date_of_delivery { get; set; }
    }
}
