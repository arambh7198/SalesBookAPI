using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SalesBookAPI.Custom
{
    public class Encrypto
    {
        //private string strEncrypted;
        //private string strDecrypted;
        //private byte[] buff;

        private string getPassword()
        {
            return getPasskey("123456"); // This is default Password for encryption and descrpt
        }
        private string getApplicationIdPassword()
        {
            return getPasskey("654321"); // This is default Password for encryption and descrpt
        }
        // 3 overloads Encrypt and Decrypt
        public string Encrypt(string TextToEncrypt,bool ForApplicationId = false)
        {
            string str = "";
            Aes aes = Aes.Create();
            aes.Key = this.GetHashSHA(ForApplicationId ? getApplicationIdPassword() : getPassword());
            aes.IV = new byte[aes.BlockSize / 8];
            byte[] bytes = Encoding.UTF8.GetBytes(TextToEncrypt);
            str = Convert.ToBase64String(aes.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length));
            return str;
        }

        public string Decrypt(string Text2Decrypt, bool ForApplicationId = false)
        {
            string str = "";
            Aes aes = Aes.Create();
            aes.Key = this.GetHashSHA(ForApplicationId ? getApplicationIdPassword() : getPassword());
            aes.IV = new byte[aes.BlockSize / 8];
            byte[] inputBuffer = Convert.FromBase64String(Text2Decrypt);
            str = Encoding.UTF8.GetString(aes.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
            return str;
        }
        
        private string getPasskey(string PassKeyString)
        {
            return Encoding.ASCII.GetString(this.GetHashMD5(PassKeyString));
        }

        private byte[] GetHashMD5(string StringToHash)
        {
            StringToHash = StringToHash.Replace(" ", "");
            byte[] hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(StringToHash));
            byte[] numArray = new byte[24];
            int index1 = 0;
            for (int index2 = 0; index2 <= numArray.Length - 1; ++index2)
            {
                numArray[index2] = hash[index1];
                ++index1;
                if (index1 >= 16)
                    index1 = 0;
            }
            return numArray;
        }

        private byte[] GetHashSHA(string StringToHash)
        {
            StringToHash = StringToHash.Replace(" ", "");
            return SHA256.Create().ComputeHash(Encoding.ASCII.GetBytes(StringToHash));
        }

        private enum PasswordTypes
        {
            StringPassword,
            PassKeyHashed,
        }

        private enum EncryptionTypes
        {
            TripleDES,
            AES,
        }

        private enum HashTypes
        {
            MD5,
            SHA256,
        }
    }
}