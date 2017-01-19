﻿using QOBDCommon.Interfaces.DAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QOBDCommon.Entities
{
    public class Safe
    {
        public Agent AuthenticatedUser { get; set; }
        public bool IsAuthenticated { get; set; }

        /*public static Agent AuthenticateUser(IDataAccessManager DataAccessComponent,string username, string password)
        {
            throw new NotImplementedException();
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            var key = "59";
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input+key));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            md5Hash.Dispose();
            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/
    }
}
