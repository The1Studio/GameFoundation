#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tls
{
    /**
     * RFC 5246 7.4.1.4.1 (in RFC 2246, there were no specific values assigned)
     */
    public class SignatureAlgorithm
    {
        public const short anonymous = 0;
        public const short rsa       = 1;
        public const short dsa       = 2;
        public const short ecdsa     = 3;

        /*
         * RFC 8422
         */
        public const short ed25519 = 7;
        public const short ed448   = 8;

        /*
         * RFC 8446 (implied from SignatureScheme values)
         * RFC 8447 reserved these values without allocating the implied names
         */
        public const short rsa_pss_rsae_sha256 = 4;
        public const short rsa_pss_rsae_sha384 = 5;
        public const short rsa_pss_rsae_sha512 = 6;
        public const short rsa_pss_pss_sha256  = 9;
        public const short rsa_pss_pss_sha384  = 10;
        public const short rsa_pss_pss_sha512  = 11;

        /*
         * RFC 8734 (implied from SignatureScheme values)
         */
        public const short ecdsa_brainpoolP256r1tls13_sha256 = 26;
        public const short ecdsa_brainpoolP384r1tls13_sha384 = 27;
        public const short ecdsa_brainpoolP512r1tls13_sha512 = 28;

        /*
         * draft-smyshlyaev-tls12-gost-suites-10
         */
        public const short gostr34102012_256 = 64;
        public const short gostr34102012_512 = 65;

        public static short GetClientCertificateType(short signatureAlgorithm)
        {
            switch (signatureAlgorithm)
            {
                case rsa:
                case rsa_pss_rsae_sha256:
                case rsa_pss_rsae_sha384:
                case rsa_pss_rsae_sha512:
                case rsa_pss_pss_sha256:
                case rsa_pss_pss_sha384:
                case rsa_pss_pss_sha512:
                    return ClientCertificateType.rsa_sign;

                case dsa:
                    return ClientCertificateType.dss_sign;

                case ecdsa:
                case ed25519:
                case ed448:
                    return ClientCertificateType.ecdsa_sign;

                case gostr34102012_256:
                    return ClientCertificateType.gost_sign256;

                case gostr34102012_512:
                    return ClientCertificateType.gost_sign512;

                default:
                    return -1;
            }
        }

        public static string GetName(short signatureAlgorithm)
        {
            switch (signatureAlgorithm)
            {
                case anonymous:
                    return "anonymous";
                case rsa:
                    return "rsa";
                case dsa:
                    return "dsa";
                case ecdsa:
                    return "ecdsa";
                case ed25519:
                    return "ed25519";
                case ed448:
                    return "ed448";
                case gostr34102012_256:
                    return "gostr34102012_256";
                case gostr34102012_512:
                    return "gostr34102012_512";
                case rsa_pss_rsae_sha256:
                    return "rsa_pss_rsae_sha256";
                case rsa_pss_rsae_sha384:
                    return "rsa_pss_rsae_sha384";
                case rsa_pss_rsae_sha512:
                    return "rsa_pss_rsae_sha512";
                case rsa_pss_pss_sha256:
                    return "rsa_pss_pss_sha256";
                case rsa_pss_pss_sha384:
                    return "rsa_pss_pss_sha384";
                case rsa_pss_pss_sha512:
                    return "rsa_pss_pss_sha512";
                case ecdsa_brainpoolP256r1tls13_sha256:
                    return "ecdsa_brainpoolP256r1tls13_sha256";
                case ecdsa_brainpoolP384r1tls13_sha384:
                    return "ecdsa_brainpoolP384r1tls13_sha384";
                case ecdsa_brainpoolP512r1tls13_sha512:
                    return "ecdsa_brainpoolP512r1tls13_sha512";
                default:
                    return "UNKNOWN";
            }
        }

        public static string GetText(short signatureAlgorithm) { return GetName(signatureAlgorithm) + "(" + signatureAlgorithm + ")"; }
    }
}
#pragma warning restore
#endif