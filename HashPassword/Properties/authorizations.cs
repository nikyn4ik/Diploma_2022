using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashPassword.Properties
{
    public class authorizations
    {
        [Key] public int id_authorization { get; set; }
        public string login { get; set; }
        public string password_hash { get; set; }
        public string FIO { get; set; }

    }

}
