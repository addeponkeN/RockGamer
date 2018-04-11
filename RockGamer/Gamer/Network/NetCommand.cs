using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockGamer.Gamer.Network
{
    public abstract class NetCommand
    {
        public NetClient Client = NetworkManager.Instance.Client;

        public abstract void Read(NetIncomingMessage inc);

    }
}
