#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Modes.Gcm
{
    using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Utilities;
    using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities;

    public class Tables4kGcmMultiplier
        : IGcmMultiplier
    {
        private byte[]  H;
        private ulong[] T;

        public void Init(byte[] H)
        {
            if (this.T == null)
                this.T = new ulong[512];
            else if (Arrays.AreEqual(this.H, H)) return;

            this.H = Arrays.Clone(H);

            // T[0] = 0

            // T[1] = H.p^7
            GcmUtilities.AsUlongs(this.H, this.T, 2);
            GcmUtilities.MultiplyP7(this.T, 2, this.T, 2);

            for (var n = 2; n < 256; n += 2)
            {
                // T[2.n] = T[n].p^-1
                GcmUtilities.DivideP(this.T, n, this.T, n << 1);

                // T[2.n + 1] = T[2.n] + T[1]
                GcmUtilities.Xor(this.T, n << 1, this.T, 2, this.T, (n + 1) << 1);
            }
        }

        public void MultiplyH(byte[] x)
        {
            //ulong[] z = new ulong[2];
            //GcmUtilities.Copy(T, x[15] << 1, z, 0);
            //for (int i = 14; i >= 0; --i)
            //{
            //    GcmUtilities.MultiplyP8(z);
            //    GcmUtilities.Xor(z, 0, T, x[i] << 1);
            //}
            //Pack.UInt64_To_BE(z, x, 0);

            var   pos = x[15] << 1;
            ulong z0  = this.T[pos + 0], z1 = this.T[pos + 1];

            for (var i = 14; i >= 0; --i)
            {
                pos = x[i] << 1;

                var c = z1 << 56;
                z1 = this.T[pos + 1] ^ ((z1 >> 8) | (z0 << 56));
                z0 = this.T[pos + 0] ^ (z0 >> 8) ^ c ^ (c >> 1) ^ (c >> 2) ^ (c >> 7);
            }

            Pack.UInt64_To_BE(z0, x, 0);
            Pack.UInt64_To_BE(z1, x, 8);
        }
    }
}
#pragma warning restore
#endif