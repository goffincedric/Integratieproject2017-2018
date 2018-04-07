using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Items
{
  [Table("tblTheme")]
  public class Theme : Item
  {
    public string ThemeName { get; set; }
    public string Description { get; set; }
  }
}
