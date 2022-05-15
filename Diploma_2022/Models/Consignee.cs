using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    public class Consignee
    {
        public int id_consignee { get; set; }

        public int id_payer { get; set; }

        public string FIO_consignee { get; set; }

        public string phone { get; set; }

        public string email { get; set; }

        public string name_consignee { get; set; }
    }
}
