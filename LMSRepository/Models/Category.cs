﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<LibraryAsset> Assets { get; set; }
        public int LibraryAssetId { get; set; }
    }
}
