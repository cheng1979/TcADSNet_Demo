using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcADSNet_Demo.Model;
using TwinCAT.TypeSystem;

namespace TcADSNet_Demo.ViewModels
{
    public class VariablesTreeViewModel : BindableBase
    {
        private PlcMembersCollection _membersCollection;
        public PlcMembersCollection MembersCollection
        {
            get { return _membersCollection; }
            set { SetProperty(ref _membersCollection, value); }
        }


        public VariablesTreeViewModel()
        {
            AdsConn.evAdsConnected      += AdsConn_evAdsConnected;
            AdsConn.evAdsDisconnected   += AdsConn_evAdsDisconnected;

            GetPlcSymbols();
            
        }

        #region Event Listener
        private void AdsConn_evAdsDisconnected(object sender, EventArgs e)
        {
            ///Update _membersCollection when ADS Disconnected
            GetPlcSymbols();
        }

        private void AdsConn_evAdsConnected(object sender, EventArgs e)
        {
            ///Update _membersCollection when ADS Connection Acquired
            GetPlcSymbols();
        }
        #endregion

        public void GetPlcSymbols()
        {
            if (AdsConn.Instance.Client.IsConnected)
            {
                ISymbolLoader symbolsColl = AdsConn.Instance.GetSymbol(); /// get plc symbols
                List<PlcMember> memberList = new List<PlcMember>();
                ///Prepare memberList
                foreach (IVirtualStructInstance item in symbolsColl.Symbols)
                {
                    ///Get member name and symbols collection (MemberInstances)
                    memberList.Add(new PlcMember(item.InstanceName, item.MemberInstances));
                }
                ///Assign member collection
                MembersCollection = new PlcMembersCollection(memberList);
            }
            else
            {
                ///ADS Client not connected
                List<PlcMember> emptyList = new List<PlcMember>();
                //emptyList.Add(new PlcMember("Empty")); ///Set an empty collection with member name as Empty
                ///Assign member collection
                MembersCollection = new PlcMembersCollection(emptyList);
            }
        }

    }///Class
}
