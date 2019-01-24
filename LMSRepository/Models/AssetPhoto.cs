using System;
using System.Collections.Generic;
using System.Text;

namespace LMSLibrary.Models
{
    public class AssetPhoto : Photo
    {
        public LibraryAsset LibraryAsset { get; set; }
        public int LibraryAssetId { get; set; }
    }
}
