using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Web_Programlama__Proje.Areas.Identity.Data;

// Add profile data for application users by adding properties to the DetilsUser class
public class DetilsUser : IdentityUser
{
    [Required]
    [Display(Name ="Müşteri Adı")]
    public string UsrAd { get; set; }

    [Required]
    [Display(Name = "Müşteri Soy Adı")]
    public string UsrSoyad{ get; set; }

}

