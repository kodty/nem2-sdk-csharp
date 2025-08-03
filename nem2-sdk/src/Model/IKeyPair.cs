namespace io.nem2.sdk.src.Model
{
    interface IKeyPair
    {
        byte[] PrivateKey { get; }

        byte[] PublicKey { get; }
    }
}
