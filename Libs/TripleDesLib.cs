using System;
using System.Security.Cryptography;
using System.Text;

namespace automotriz_webapi.Libs
{
    public class TripleDesLib
    {
        private readonly string Key;

        public TripleDesLib(string key)
        {
            this.Key = key;
        }
    
        public string Encrypt(string TextToEncrypt)
        {
            byte[] MyEncryptedArray = UTF8Encoding.UTF8.GetBytes(TextToEncrypt);
            var MyMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(this.Key));
    
            MyMD5CryptoService.Clear();
    
            var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();
    
            MyTripleDESCryptoService.Key = MysecurityKeyArray;
            MyTripleDESCryptoService.Mode = CipherMode.ECB;
            MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
    
            var MyCrytpoTransform = MyTripleDESCryptoService.CreateEncryptor();
    
            byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyEncryptedArray, 0, MyEncryptedArray.Length);
    
            MyTripleDESCryptoService.Clear();
    
            return Convert.ToBase64String(MyresultArray, 0, MyresultArray.Length);
        }
    
        public string Decrypt(string TextToDecrypt)
      {
         byte[] MyDecryptArray = Convert.FromBase64String
            (TextToDecrypt);
 
         var MyMD5CryptoService = new MD5CryptoServiceProvider();
 
         byte[] MysecurityKeyArray = MyMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(this.Key));
 
         MyMD5CryptoService.Clear();
 
         var MyTripleDESCryptoService = new TripleDESCryptoServiceProvider();
 
         MyTripleDESCryptoService.Key = MysecurityKeyArray;
 
         MyTripleDESCryptoService.Mode = CipherMode.ECB;
 
         MyTripleDESCryptoService.Padding = PaddingMode.PKCS7;
 
         var MyCrytpoTransform = MyTripleDESCryptoService.CreateDecryptor();
 
         byte[] MyresultArray = MyCrytpoTransform.TransformFinalBlock(MyDecryptArray, 0, MyDecryptArray.Length);
 
         MyTripleDESCryptoService.Clear();
 
         return UTF8Encoding.UTF8.GetString(MyresultArray);
      }
    }
}