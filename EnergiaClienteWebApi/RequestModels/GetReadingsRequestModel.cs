using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergiaClienteWebApi.RequestModels
{
    public class GetReadingsRequestModel
    {
        public int habitation { get; set; }
        public int quantity { get; set; }
    }
}
