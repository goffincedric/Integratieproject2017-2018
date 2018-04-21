using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Items
{

    [Table("tblHashtag")]
  public class Hashtag
  {
    [Key] public int Id { get; set; }
    public String HashTag { get; set; }

    public List<Record> Records { get; set; }

    public Hashtag()
    {

    }

    public Hashtag(string hashTag)
    {
     HashTag = hashTag;
    }
  }
}
