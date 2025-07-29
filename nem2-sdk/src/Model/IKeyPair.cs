namespace io.nem2.sdk.src.Model
{
    /// <summary>
    /// Interface IKeyPair
    /// </summary>
    interface IKeyPair
    {
        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>The private key.</value>
        byte[] PrivateKey { get; }
        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        byte[] PublicKey { get; }
    }
}
