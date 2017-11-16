using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KittensMicroservice.Viewmodels
{
    public class GetTokenRequest
    {
        [Required]
        [MaxLength(32)]
        public string Username { get; set; }
        [Required]
        [MaxLength(32)]
        [Compare(nameof(Username))]
        public string Password { get; set; }
        //vague description in assignment - is this field required or optional?
        [Range(10,300)]
        public int? ExpiresIn { get; set; }
    }
}
