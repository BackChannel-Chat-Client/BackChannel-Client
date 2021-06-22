using BackChannel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Binary;

namespace BackChannel.Classes
{
    public class Packet
    {
        public UInt32 PacketSize { get; set; }
        public UInt32 PacketID { get; set; }
        public UInt32 ChannelID { get; set; }
        public byte RequestType { get; set; }
        public byte[] AuthKey { get; set; }
        public byte[] RequestBody { get; set; }
        public Socket PacketSocket { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }

        public static byte[] None = { 0x0 };
        public static List<Packet> PacketQueue { get; set; }

        public static byte[] ToByteArray(string data)
        {
            return Encoding.ASCII.GetBytes($"{data}\x0");
        }

        public void GeneratePID()
        {
            uint ID = (uint)new Random().Next(sizeof(UInt32));
            PacketID = ID;
        }

        public void SetRequestType(RequestType type)
        {
            RequestType = (byte)type;
        }

        public void GetPacketSize()
        {
            PacketSize = (uint)(13 + AuthKey.Length + RequestBody.Length + 1);
        }

        public byte[] GetPacketAsByteArray()
        {
            IEnumerable<byte> packet = BitConverter.GetBytes(PacketSize)
                                        .Concat(BitConverter.GetBytes(PacketID))
                                        .Concat(BitConverter.GetBytes(ChannelID))
                                        .Concat(BitConverter.GetBytes(RequestType))
                                        .Concat(AuthKey)
                                        .Concat(RequestBody);
            return packet.ToArray();
        }

        public void SendPacket()
        {
            IPAddress ip = IPAddress.Parse(ServerIP);
            IPEndPoint localEndPoint = new IPEndPoint(ip, ServerPort);
            PacketSocket.Connect(localEndPoint);
            PacketSocket.Send(GetPacketAsByteArray());
        }

        //public static ServerError GetLastError(string AuthKey)
        //{
        //    Packet pack = new Packet();
        //    pack.ChannelID = 0;
        //    pack.RequestType = 0x00;
        //    pack.AuthKey = Packet.ToByteArray($"{AuthKey}\x00");
        //    pack.RequestBody = Packet.ToByteArray("\x00");
        //    pack.GetPacketSize();
        //    PacketQueue.Add(pack);
        //    pack.SendPacket();
        //    string res = pack.RecvResponse();
        //    return (ServerError)uint.Parse(res, System.Globalization.NumberStyles.AllowHexSpecifier);
        //}

        public Response RecvResponse()
        {
            byte[] Size = new byte[sizeof(UInt32)];
            PacketSocket.Receive(Size);
            byte[] ID = new byte[sizeof(UInt32)];
            PacketSocket.Receive(ID);
            byte[] Status = new byte[sizeof(UInt32)];
            PacketSocket.Receive(Status);
            byte[] Body = new byte[BitConverter.ToUInt32(Size) - 12];
            PacketSocket.Receive(Body);
            //PacketQueue.Remove(this);

            Response res = new Response();
            res.PacketSize = (UInt32)BitConverter.ToUInt32(Size);
            res.PacketID = (UInt32)BitConverter.ToUInt32(ID);
            res.ResponseStatus = (UInt32)BitConverter.ToUInt32(Status);
            res.ResponseBody = Body;

            return res;
        }

        public Packet()
        {
            PacketSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            Random rand = new Random();

            PacketID = (uint)rand.Next(sizeof(UInt32));
        }
    }
}
