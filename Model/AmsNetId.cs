using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public class AmsNetId : BindableBase
    {
        public String NetId { get; set; }

        public AmsNetId()
        {
            NetId = "";
        }

        public AmsNetId(String id)
        {
            NetId = id;
        }

    }///Class
}
