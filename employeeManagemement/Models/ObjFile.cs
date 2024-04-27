using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace employeeManagemement.Models
{
    public class ObjFile
    {
        public List<IFormFile>? Files { get; set; }
        public string ?File { get; set; }
        public long Size { get; set; }
        public string ?Type { get; set; }
    }
}
