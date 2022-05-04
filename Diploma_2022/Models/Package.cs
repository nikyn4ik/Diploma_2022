using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    public class Package
    {
        public int id_package { get; set; }

        public int   id_order { get; set; }

        public string mark_package { get; set; }

        public int type_model { get; set; }

        public DateTime date_package { get; set; }
    }
}
