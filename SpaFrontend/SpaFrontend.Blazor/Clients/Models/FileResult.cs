using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaFrontend.Blazor.Clients.Models
{
    public class FileResult
    {
        public string FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
    }
}
