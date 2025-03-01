using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{
    public class FileOptions
    {
        public required int MaxSizeMB { get; set; }
        public required List<string> AcceptedFormats { get; set; }
    }
}
