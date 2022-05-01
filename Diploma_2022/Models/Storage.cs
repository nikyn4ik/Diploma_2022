using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    class Storage
    {
        public int id_storage { get; set; }
        public string name_storage { get; set; }
        public string address { get; set; }
        public string phone_storage { get; set; }
        public string FIO_responsible_person { get; set; }
        public string remainder { get; set; }
        public DateTime date_add_storage { get; set; }
    }
}
