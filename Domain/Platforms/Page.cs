using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PB.BL.Domain.Platform
{
    [Table("tblPage")]
    public class Page
    {
        [Key] public int PageId { get; set; }

        [Required] public string PageName { get; set; }

        [Required] public string Title { get; set; }

        public List<Tag> Tags { get; set; }

        public int SubplatformId { get; set; }

        [Required] public Subplatform Subplatform { get; set; }

        public override bool Equals(object obj)
        {
            var page = obj as Page;
            return page != null &&
                   PageName == page.PageName &&
                   EqualityComparer<Subplatform>.Default.Equals(Subplatform, page.Subplatform);
        }

        public override int GetHashCode()
        {
            var hashCode = -562850145;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PageName);
            hashCode = hashCode * -1521134295 + EqualityComparer<Subplatform>.Default.GetHashCode(Subplatform);
            return hashCode;
        }
    }
}