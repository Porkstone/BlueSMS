namespace BlueSMS
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// This class encrypt's and decrypts config data using the MS DpApi
    /// http://msdn.microsoft.com/en-us/library/ms995355.aspx
    /// </summary>
    public static class ConfigProtect
    {
        // DataProtectionScope.CurrentUser. The result can be only decrypted by the same user account on the same machine.
        // DataProtectionScope.LocalMachine date can be decrypted by any process running on the same server
        // Note: If using DataProtectionScope.CurrentUser in IIS you might need to set AppPool.ProcessModel.LoadUserProfile=True to get it working
        const DataProtectionScope ScopeSetting = DataProtectionScope.CurrentUser;

        static byte[] s_aditionalEntropy = { 9, 8, 7, 6, 5 };

        public static string Encrypt(string text)
        {
            return GetBase64String(Protect(GetBytes(text)));
        }

        public static string Decrypt(string text)
        {
            return GetString(Unprotect(GetBytesFromBase64(text)));
        }

        private static byte[] Protect(byte[] data)
        {

            return ProtectedData.Protect(data, s_aditionalEntropy, ScopeSetting);
        }

        private static byte[] Unprotect(byte[] data)
        {
            return ProtectedData.Unprotect(data, s_aditionalEntropy, ScopeSetting);
        }

        private static byte[] GetBytesFromBase64(string str)
        {
            return Convert.FromBase64String(str);
        }

        private static string GetBase64String(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

    }
}
