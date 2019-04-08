using System;
using System.Collections.Generic;
using System.Text;

namespace MilNet.Services.Models
{
    public class DocumentationOptions
    {
        public string Version { get; set; }
        public bool EnableSecurity { get; set; }
        public string XmlCommentsFilePath { get; set; }
        public bool DescribeAllEnumsAsStrings { get; set; }
        public bool DescribeAllParametersInCamelCase { get; set; }
        public bool DescribeStringEnumsInCamelCase { get; set; }
    }
}
