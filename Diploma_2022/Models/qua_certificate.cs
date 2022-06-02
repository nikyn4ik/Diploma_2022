using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    public class qua_certificate
    {
        public int id_qua_certificate { get; set; }

        public string standard_per_mark { get; set; }

        public string product_standard { get; set; }

        public DateTime date_add_certificate { get; set; }

        public string manufacturer { get; set; }

        public int id_cert_directory { get; set; }
    }
}
