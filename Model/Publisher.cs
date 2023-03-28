using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcADSNet_Demo.Model
{
    public static class Publisher
    {
        public static event EventHandler<String> evPublisher;

        public static void Publish(String msg)
        {
            evPublisher?.Invoke(null, msg);
        }
    }
}
