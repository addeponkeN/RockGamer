using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RockGamer.Gamer.Network
{
    public class NetworkManager
    {
        static NetworkManager instance = new NetworkManager();
        public static NetworkManager Instance => Instance;

        public NetClient Client;

        public NetOutgoingMessage CreateMessage() { return Client.CreateMessage(); }

        public void Start()
        {
            Client = new NetClient(new NetPeerConfiguration(Globals.ProjectName));
            Client.Start();
            new Thread(NetworkUpdate).Start();
        }

        public void NetworkUpdate()
        {

            while(true)
            {
                Thread.Sleep(1);
                NetIncomingMessage inc;

                if((inc = Client.ReadMessage()) == null) continue;

                switch(inc.MessageType)
                {
                    case NetIncomingMessageType.Error:
                        Console.WriteLine("NIM Error : " + inc.ReadString());
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        break;

                    case NetIncomingMessageType.ConnectionApproval:
                        break;

                    case NetIncomingMessageType.Data:
                        ReadData(inc);
                        break;

                    case NetIncomingMessageType.VerboseDebugMessage:
                        Console.WriteLine("NIM Verbose : " + inc.ReadString());
                        break;

                    case NetIncomingMessageType.DebugMessage:
                        Console.WriteLine("NIM Debug : " + inc.ReadString());
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        Console.WriteLine("NIM Warning : " + inc.ReadString());
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine("NIM ErrorMsg : " + inc.ReadString());
                        break;

                    default:
                        Console.WriteLine(inc.MessageType + " - no such inc.MessageType");
                        break;
                }

            }

        }


        void ReadData(NetIncomingMessage inc)
        {
            string commandName = "AccountLoginCommand";
            string instance = $"{Globals.ProjectName}.Gamer.Network.{commandName}";

            NetCommand command = Activator.CreateInstance(Type.GetType(instance)) as NetCommand;
            command.Read(inc);
        }

    }
}
