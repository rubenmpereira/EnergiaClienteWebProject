using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergiaClienteWebApi.RequestModels
{
    public class CalculateInvoiceRequestModel
    {
        public int habitation { get; set; }
        public int vazio { get; set; }
        public int ponta { get; set; }
        public int cheias { get; set; }
    }
}
