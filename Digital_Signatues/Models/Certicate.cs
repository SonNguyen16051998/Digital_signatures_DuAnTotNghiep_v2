using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using System.Collections.Generic;
using System.IO;

namespace Digital_Signatues.Models
{
    public class Certicate
    {
        private string path = "";
        private string password = "";
        private AsymmetricKeyParameter akp;
        private Org.BouncyCastle.X509.X509Certificate[] chain;
        public Org.BouncyCastle.X509.X509Certificate[] Chain
        {
            get { return chain; }
        }
        public AsymmetricKeyParameter Akp
        {
            get { return akp; }
        }

        public string Path
        {
            get { return path; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private void processCert()
        {
            Pkcs12Store pk12;

            //First we'll read the certificate file
            pk12 = new Pkcs12Store(new FileStream(this.Path, FileMode.Open, FileAccess.Read), this.password.ToCharArray());

            //then Iterate throught certificate entries to find the private key entry
            string keyAlias = null;

            foreach (string name in pk12.Aliases)
            {
                if (pk12.IsKeyEntry(name))
                {
                    keyAlias = name;
                    break;
                }
            }
            this.akp = pk12.GetKey(keyAlias).Key;
            X509CertificateEntry[] ce = pk12.GetCertificateChain(keyAlias);
         
            this.chain = new Org.BouncyCastle.X509.X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;
            }   
        }
        public Certicate()
        { }
        public Certicate(string cpath)
        {
            this.path = cpath;
            this.processCert();
        }
        public Certicate(string cpath, string cpassword)
        {
            this.path = cpath;
            this.Password = cpassword;
            this.processCert();
        }
    }
}
