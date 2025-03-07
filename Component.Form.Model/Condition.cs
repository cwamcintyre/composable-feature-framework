using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Form.Model
{

    public class Condition
    {
        public string Id { get; set; } = string.Empty;
        public string Expression { get; set; } = string.Empty;
        public string NextPageId { get; set; } = string.Empty;
    }
}
