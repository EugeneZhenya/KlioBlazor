using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlioBlazor.Shared.Enums
{
    public enum AgeCategory
    {
        [Display(GroupName = "Green", Name = "0+", Order = 0, Description = "0plus")]
        Zero = 0,
        [Display(GroupName = "Gold", Name = "12+", Order = 12, Description = "12plus")]
        Twelve = 12,
        [Display(GroupName = "Orange", Name = "16+", Order = 16, Description = "16plus")]
        Sixteen = 16,
        [Display(GroupName = "Red", Name = "18+", Order = 18, Description = "18plus")]
        Eighteen = 18
    }
}
