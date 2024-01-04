using System;

namespace Diploma_2022.Models
{
    internal class Certificates
    {
        public int id_qua_certificate { get; set; }
        public string standard_per_mark { get; set; }
        public string manufacturer { get; set; }
        public string product_standard { get; set; }
        public DateTime date_add_certificate { get; set; }
    }
}
