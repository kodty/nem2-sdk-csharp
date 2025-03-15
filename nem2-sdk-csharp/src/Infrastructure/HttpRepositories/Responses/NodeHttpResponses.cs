using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.Buffers.Model
{
    public class NodeHealth
    {
        public Status Status { get; set; }
    }

    public class Status
    {
        public string ApiNode { get; set; }

        public string Db { get; set; }
    }

    public class NodeInfo : NodePeer
    {

        public string NodePublicKey { get; set; }
    }

    public class NodePeer
    {
        public int Version { get; set; }

        public string PublicKey { get; set; }

        public string NetworkGenerationHashSeed { get; set; }

        public int Roles { get; set; }

        public int Port { get; set; }

        public int NetworkIdentifier { get; set; }

        public string FriendlyName { get; set; }

        public string Host { get; set; }
    }

    public class NodeStorage
    {
        public int NumBlocks { get; set; }

        public int NumTransactions { get; set; }

        public int NumAccounts { get; set; }
    }

    public class CommunicationTimestamps
    {
        public ulong SendTimestamp { get; set; }

        public ulong ReceiveTimestamp { get; set; }
    }

    public class NodeTime
    {
        public CommunicationTimestamps CommunicationTimestamps { get; set; }
    }

    public class Deployment
    {
        public string DeploymentTool { get; set; }

        public string DeploymentToolVersion { get; set; }

        public string LastUpdatedDate { get; set; }
    }

    public class NodeRESTVersion
    {
        public ServerInfo ServerInfo { get; set; }
    }

    public class ServerInfo
    {
        public string RestVersion { get; set; }

        public string SdkVersion { get; set; }

        public Deployment Deployment { get; set; }
    }

    public class NodeUnlockedAccounts
    {
        public List<string> UnlockedAccount { get; set; }
    }
}
