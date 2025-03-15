using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string role { get; set; }

    }
}
