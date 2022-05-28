using System;

namespace Diploma_2022.Models
{
    public class Package
    {
        public int id_package { get; set; }

        public int  id_order { get; set; }

        public string mark_package { get; set; }

        public string type_model { get; set; }

        public DateTime date_package { get; set; }
    }
}
