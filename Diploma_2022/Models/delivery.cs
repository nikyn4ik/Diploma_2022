using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    public class Delivery
    {
        public int id_delivery { get; set; }

        public DateTime date_of_delivery { get; set; }

        public int id_order { get; set; }

        public string early_delivery { get; set; }
    }
}
