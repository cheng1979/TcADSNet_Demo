using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinCAT.TypeSystem;

namespace TcADSNet_Demo.Model
{
    public class PlcMembersCollection
    {
        public ReadOnlyCollection<PlcMember> Collection{ get; set; }

        public PlcMembersCollection(List<PlcMember> membersList)
        {
            this.Collection = new ReadOnlyCollection<PlcMember>(membersList);
        }


    }///Class
}
