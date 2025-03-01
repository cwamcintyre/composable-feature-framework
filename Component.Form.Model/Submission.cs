using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{
    public class Submission
    {
        public required string Method { get; set; }
        public required string Endpoint { get; set; }
    }
}
