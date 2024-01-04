using System.Security.Cryptography;
using System.Text;

namespace Diploma_2022
{
    internal class Users
    {
        public int id_authorization { get; set; }
        public string login { get; set; }
        private string passwordHash;

        public string Password
        {
            get { return passwordHash; }
            set { passwordHash = HashPassword(value); }
        }

        public static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}