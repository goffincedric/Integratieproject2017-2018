using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblTag")]
    public class Tag
    {
        [Key] public int TagId { get; set; }

        [Required] public string Name { get; set; }

        public string Text { get; set; }
        public string Text2 { get; set; }

        public int PageId { get; set; }

        [Required] public Page Page { get; set; }

        public override bool Equals(object obj)
        {
            var tag = obj as Tag;
            return tag != null &&
                   Name == tag.Name &&
                   EqualityComparer<Page>.Default.Equals(Page, tag.Page);
        }

        public override int GetHashCode()
        {
            var hashCode = 140442508;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Page>.Default.GetHashCode(Page);
            return hashCode;
        }
    }
}