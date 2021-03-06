﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DevMentor.Data.Cryptography
{
    public class Hash
    {
        public static string ComputeHash(string msg)
        {
            string result = string.Empty;
            var md5 = new MD5CryptoServiceProvider();
            var sha1 = new SHA1CryptoServiceProvider();
            var sha256 = new SHA256CryptoServiceProvider();
            var sha384 = new SHA384CryptoServiceProvider();
            var sha512 = new SHA512CryptoServiceProvider();
            var ripemd160 = new RIPEMD160Managed();

            var source = System.Text.UTF8Encoding.Default.GetBytes(msg);

            var algorithms = new Dictionary<string, HashAlgorithm>();
            algorithms["md5"] = md5;
            algorithms["sha1"] = sha1;
            algorithms["sha256"] = sha256;
            algorithms["sha384"] = sha384;
            algorithms["sha512"] = sha512;
            algorithms["ripemd160"] = ripemd160;

            result = Convert.ToBase64String(sha512.ComputeHash(source));
            return result;
        }
    }
}
