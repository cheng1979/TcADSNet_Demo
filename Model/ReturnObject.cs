using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public class ReturnObject<T>
    {
        public T Data { get; set; }
        public bool Successfully { get; set; }

        public ReturnObject()
        {
            Successfully = false;
        }
        public ReturnObject(T data, bool successfully)
        {
            Data = data;
            Successfully = successfully;
        }
    }
}
