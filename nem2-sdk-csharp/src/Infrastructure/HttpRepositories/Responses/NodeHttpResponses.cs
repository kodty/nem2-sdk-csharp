using Newtonsoft.Json;

namespace io.nem2.sdk.src.Infrastructure.Buffers.Model
{
    public class NodeHealth
    {
        [JsonProperty("status")]
        public Status Status { get; set; }
    }

    public class Status
    {
        [JsonProperty("apiNode")]
        public string ApiNode { get; set; }

        [JsonProperty("db")]
        public string Db { get; set; }
    }

    public class NodeInfo
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        [JsonProperty("networkGenerationHashSeed")]
        public string NetworkGenerationHashSeed { get; set; }

        [JsonProperty("roles")]
        public int Roles { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("networkIdentifier")]
        public int NetworkIdentifier { get; set; }

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("nodePublicKey")]
        public string NodePublicKey { get; set; }
    }

    public class NodePeer
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        [JsonProperty("networkGenerationHashSeed")]
        public string NetworkGenerationHashSeed { get; set; }

        [JsonProperty("roles")]
        public int Roles { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("networkIdentifier")]
        public int NetworkIdentifier { get; set; }

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("nodePublicKey")]
        public string NodePublicKey { get; set; }
    }

    public class NodeStorage
    {
        [JsonProperty("numBlocks")]
        public int NumBlocks { get; set; }

        [JsonProperty("numTransactions")]
        public int NumTransactions { get; set; }

        [JsonProperty("numAccounts")]
        public int NumAccounts { get; set; }
    }

    public class CommunicationTimestamps
    {
        [JsonProperty("sendTimestamp")]
        public long SendTimestamp { get; set; }

        [JsonProperty("receiveTimestamp")]
        public long ReceiveTimestamp { get; set; }
    }

    public class NodeTime
    {
        [JsonProperty("communicationTimestamps")]
        public CommunicationTimestamps CommunicationTimestamps { get; set; }
    }

    public class Deployment
    {
        [JsonProperty("deploymentTool")]
        public string DeploymentTool { get; set; }

        [JsonProperty("deploymentToolVersion")]
        public string DeploymentToolVersion { get; set; }

        [JsonProperty("lastUpdatedDate")]
        public string LastUpdatedDate { get; set; }
    }

    public class NodeRESTVersion
    {
        [JsonProperty("serverInfo")]
        public ServerInfo ServerInfo { get; set; }
    }

    public class ServerInfo
    {
        [JsonProperty("restVersion")]
        public string RestVersion { get; set; }

        [JsonProperty("sdkVersion")]
        public string SdkVersion { get; set; }

        [JsonProperty("deployment")]
        public Deployment Deployment { get; set; }
    }

    public class NodeUnlockedAccounts
    {
        [JsonProperty("unlockedAccount")]
        public List<string> UnlockedAccount { get; set; }
    }


}
