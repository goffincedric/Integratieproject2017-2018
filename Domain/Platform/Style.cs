using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class Style
  {
    [Key]
    public int StyleId { get; set; }
    public string Font { get; set; }
    public string ThemeHEXColor1 { get; set; }
    public string ThemeHEXColor2 { get; set; }

  }
}
