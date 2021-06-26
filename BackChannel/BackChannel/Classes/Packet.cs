using BackChannel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BackChannel.Classes
{
    /// <summary>
    /// Used to send/receive BCAPI calls.<br></br>
    /// </summary>
    public class Packet
    {
        // Packet Properties
        /// <summary>
        /// The size of the packet.<br></br>
        /// Set by using GetPacketSize() once all other info is set.
        /// </summary>
        private UInt32 PacketSize { get; set; }

        /// <summary>
        /// The ID of the packet.<br></br>
        /// Used by the client and server to keep track of what calls end up where.
        /// </summary>
        public UInt32 PacketID { get; set; }

        /// <summary>
        /// The channel ID that the call is in.
        /// </summary>
        public UInt32 ChannelID { get; set; }

        /// <summary>
        /// The request to make, see request documentation or the RequestTypes enum.
        /// </summary>
        public byte RequestType { get; set; }

        /// <summary>
        /// The authentication key used to make sure the api call is being made by an authorized user.
        /// </summary>
        public byte[] AuthKey { get; set; }

        /// <summary>
        /// The body of the request.
        /// </summary>
        public byte[] RequestBody { get; set; }

        /// <summary>
        /// The SSL Stream used to send/receive packets from the server.
        /// </summary>
        public SslStream PacketStream { get; set; }

        /// <summary>
        /// The IP or Domain of the server.
        /// </summary>
        public string ServerIP { get; set; }

        /// <summary>
        /// The port to contact the server on.
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        /// Sets if the packet allows the server to have a self signed cert.
        /// </summary>
        public bool AllowSelfSigned { get; set; }

        /// <summary>
        /// If there is a connection error, this is set to tell what the error was.
        /// </summary>
        public ConnectionError connectionError { get; set; }



        // Packet static helper functions/data
        /// <summary>
        /// Used to set a property to a nullbyte, indicating an empty property.
        /// </summary>
        public static byte[] None = { 0x0 };

        /// <summary>
        /// The packet queue for the client, holds all packets waiting on a response.<br></br>
        /// Currently unused.
        /// </summary>
        public static List<Packet> PacketQueue { get; set; }

        /// <summary>
        /// Takes a string and changes it to a byte array for setting packet properties.<br></br>
        /// Ends it with a nullbyte.
        /// </summary>
        /// <param name="data">The string to be converted.</param>
        /// <returns></returns>
        public static byte[] ToByteArray(string data)
        {
            return Encoding.ASCII.GetBytes($"{data}\x0");
        }



        // Packet object helper functions
        /// <summary>
        /// Creates a random Packet ID for the current packet.<br></br>
        /// Sets it automatically.
        /// </summary>
        public void GeneratePID()
        {
            uint ID = (uint)new Random().Next(sizeof(UInt32));
            PacketID = ID;
        }

        /// <summary>
        /// Sets the current packet's request type from the RequestType Enum.
        /// </summary>
        /// <param name="type">The type to set.</param>
        public void SetRequestType(RequestType type)
        {
            RequestType = (byte)type;
        }

        /// <summary>
        /// Gets the size of the whole packet.<br></br>
        /// Shouldn't be called until all other packet properties are set.
        /// </summary>
        public void GetPacketSize()
        {
            PacketSize = (uint)(13 + AuthKey.Length + RequestBody.Length + 1);
        }



        // Send/Recv public functions
        /// <summary>
        /// Sends the packet over a secured SSl stream.
        /// </summary>
        /// <returns>True on succes, False on failure.<br></br>If False, check Packet.connectionError for more info.</returns>
        public bool SendPacket()
        {
            connectionError = ConnectionError.None;
            TcpClient client = new TcpClient(ServerIP, ServerPort);
            PacketStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

            PacketStream.AuthenticateAsClient(ServerIP);
            if (!PacketStream.IsAuthenticated)
            {
                return false;
            }

            PacketStream.Write(GetPacketAsByteArray());

            return true;
        }

        /// <summary>
        /// Gets a Response packet from the server.
        /// </summary>
        /// <returns>A Response object with the info received from the packet.</returns>
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

            Response res = new Response();
            res.PacketSize = (UInt32)BitConverter.ToUInt32(Size);
            res.PacketID = (UInt32)BitConverter.ToUInt32(ID);
            res.ResponseStatus = (UInt32)BitConverter.ToUInt32(Status);
            res.ResponseBody = Body;

            return res;
        }



        // Send/Recv private functions
        /// <summary>
        /// Converts the whole packet into a byte array for sending to the server.
        /// </summary>
        /// <returns>A byte array with all the packet details.</returns>
        private byte[] GetPacketAsByteArray()
        {
            IEnumerable<byte> packet = BitConverter.GetBytes(PacketSize)
                                        .Concat(BitConverter.GetBytes(PacketID))
                                        .Concat(BitConverter.GetBytes(ChannelID))
                                        .Concat(BitConverter.GetBytes(RequestType))
                                        .Concat(AuthKey)
                                        .Concat(RequestBody);
            return packet.ToArray();
        }

        /// <summary>
        /// Validated the server's certificate before sending info over the SSL Stream
        /// </summary>
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
    }
}
