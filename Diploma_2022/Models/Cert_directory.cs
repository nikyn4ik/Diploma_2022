using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma_2022.Models
{
    public class Cert_directory
    {
        public int id_cert_directory { get; set; }

        public int id_qua_certificate { get; set; }

        public int min { get; set; }

        public int max { get; set; }

        public string units { get; set; }

        public string properties_cert { get; set; }
    }
}
