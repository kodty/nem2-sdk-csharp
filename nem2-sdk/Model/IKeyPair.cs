namespace io.nem2.sdk.Model
{
    interface IKeyPair
    {
        byte[] PrivateKey { get; }

        byte[] PublicKey { get; }
    }
}
