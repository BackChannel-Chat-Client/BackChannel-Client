using BackChannel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Buffers.Binary;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

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
        public SslStream PacketStream { get; set; }
        public string ServerIP { get; set; }
        public bool AllowSelfSigned { get; set; }
        public ConnectionError connectionError { get; set; }
        public int ServerPort { get; set; }

        public static byte[] None = { 0x0 };
        public static List<Packet> PacketQueue { get; set; }

        // Helper functions
        public static byte[] ToByteArray(string data)
        {
            return Encoding.ASCII.GetBytes($"{data}\x0");
        }

        // Creation Functions
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

        // Send / Receive
        public bool SendPacket()
        {
            TcpClient client = new TcpClient(ServerIP, ServerPort);
            //client.Connect(ServerIP, ServerPort);

            PacketStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            PacketStream.AuthenticateAsClient(ServerIP);
            if (!PacketStream.IsAuthenticated)
            {
                return false;
            }

            PacketStream.Write(GetPacketAsByteArray());

            return true;
        }
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if(sslPolicyErrors != SslPolicyErrors.None)
            {
                if(sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    if (!AllowSelfSigned)
                    {
                        connectionError = ConnectionError.SelfSigned;
                        return false;
                    }
                    return true;
                }
                connectionError = ConnectionError.CertError;
                return false;
            }

            return true;
        }
        public Response RecvResponse()
        {
            byte[] Size = new byte[sizeof(UInt32)];
            PacketStream.Read(Size);
            byte[] ID = new byte[sizeof(UInt32)];
            PacketStream.Read(ID);
            byte[] Status = new byte[sizeof(UInt32)];
            PacketStream.Read(Status);
            byte[] Body = new byte[BitConverter.ToUInt32(Size) - 12];
            PacketStream.Read(Body);
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

        }
    }
}
