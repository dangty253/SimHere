using System;
using System.Collections.Generic;

namespace SimhereApp.Portable.Models
{
    public class GoogleSheetFileRootResponse
    {
        public string NextPageToken { get; set; }
        public List<GoogleSheetFileListResponse> Files { get; set; }
    }
}
