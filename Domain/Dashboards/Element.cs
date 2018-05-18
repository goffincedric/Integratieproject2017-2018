﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PB.BL.Domain.Items;

namespace PB.BL.Domain.Dashboards
{
    [Table("tblElement")]
    public class Element
    {
        [Key] public int ElementId { get; set; }

        [Required] public int X { get; set; }

        [Required] public int Y { get; set; }

        [Required] public int Width { get; set; }

        [Required] public int Height { get; set; }

        public bool IsDraggable { get; set; }
        public GraphType GraphType { get; set; }
        public bool IsFinished { get; set; }

        public int ZoneId { get; set; }
        public Zone Zone { get; set; }

        public List<Item> Items { get; set; }
        //public List<Comparison> Comparison { get; set; }
    }
}