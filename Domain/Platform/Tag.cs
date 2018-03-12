﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
  [Table("tblTag")]
  public class Tag
  {
    [Key]
    public int TagId { get; set; }
    public string HtmlId { get; set; }
    public List<string> HtmlClasses { get; set; }
    [Required]
    public string NameObject { get; set; }
    public string Text { get; set; }
  }
}