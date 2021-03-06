﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CERTENROLLLib;
using Pluralsight.Crypto;
using System.IO;

namespace Lime.Transport.Tcp.UnitTests
{
    public static class CertificateUtil
    {
        private static readonly ConcurrentDictionary<string, X509Certificate2> CertificateCache = new ConcurrentDictionary<string, X509Certificate2>();

        /// <summary>
        /// Creates a self-signed certificate
        /// http://stackoverflow.com/questions/13806299/how-to-create-a-self-signed-certificate-using-c
        /// </summary>
        /// <param name="subjectName"></param>       
        /// <returns></returns>
        public static X509Certificate2 CreateSelfSignedCertificate(params string[] commonNames)
        {
            using (var ctx = new CryptContext())
            {
                ctx.Open();

                var nameBuilder = new StringBuilder();
                foreach (var commonName in commonNames)
                {
                    nameBuilder.AppendLine($"CN={commonName}");
                }

                var certificate = ctx.CreateSelfSignedCertificate(
                    new SelfSignedCertProperties
                    {
                        IsPrivateKeyExportable = true,
                        KeyBitLength = 4096,
                        Name = new X500DistinguishedName(nameBuilder.ToString(), X500DistinguishedNameFlags.UseNewLines),
                        ValidFrom = DateTime.Today.AddDays(-1),
                        ValidTo = DateTime.Today.AddYears(1)
                    });

                return certificate;
            }
        }

        public static X509Certificate2 ReadFromFile(string filepath)
        {
            var bytes = File.ReadAllBytes(filepath);
            var certificate = new X509Certificate2();
            certificate.Import(bytes);
            return certificate;
        }
    }
}
