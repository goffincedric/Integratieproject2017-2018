using PB.BL.Domain.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB.BL.Domain.Platform
{
  public class SubPlatform
  {
    [Key]
    public int SubplatformId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string URL { get; set; }
    public string SourceAPI { get; set; }
    public string SiteIconURL { get; set; }
    public DateTime DateOnline { get; set; }
    [Required]
    public Style Style { get; set; }
    public List<SubplatformSetting> Settings { get; set; }
    public List<Page> Pages { get; set; }
    public List<T> Admins { get; set; }
  }
}
