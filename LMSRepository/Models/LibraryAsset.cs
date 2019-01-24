﻿using System;

namespace LMSLibrary.Models
{
    public class LibraryAsset
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public Status Status { get; set; }
        public decimal Cost { get; set; }
        public DateTime Added { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; }
        public string Description { get; set; }
        public AssetPhoto Photo { get; set; }
        public AssetType AssetType { get; set; }
        public Author Author { get; set; }
        public string ISBN { get; set; }
        public string DeweyIndex { get; set; }
        public Category Category { get; set; }
    }
}