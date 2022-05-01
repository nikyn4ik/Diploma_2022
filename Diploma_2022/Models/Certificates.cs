using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    //internal class Certificates
    public class Certificates
    {
        public Certificates()
    {

    }
        public Certificates
           (int d_qua_cert,
            string standard_mark,
            string product_stand,
            DateTime date_add_cer)
        {

            id_qua_certificate = d_qua_cert;
            standard_per_mark = standard_mark;
            product_standard = product_stand;
            date_add_certificate = date_add_cer;
        }
        public int id_qua_certificate { get; set; }

        public string standard_per_mark { get; set; }

        public string product_standard { get; set; }

        public DateTime date_add_certificate { get; set; }
    }
}
