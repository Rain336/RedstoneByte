using System;
using System.Runtime.InteropServices;

namespace RedstoneByte.Native
{
    internal static class OpenSSL
    {
        public const string DllName = "libeay32";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_CIPHER_CTX_new")]
        internal static extern IntPtr NewCipher();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_EncryptInit_ex")]
        internal static extern int InitEncryptingCipher(IntPtr handle, IntPtr cipher, IntPtr impl, IntPtr key, IntPtr iv);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_DecryptInit_ex")]
        internal static extern int InitDecryptingCipher(IntPtr handle, IntPtr cipher, IntPtr impl, IntPtr key, IntPtr iv);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_aes_128_cfb8")]
        internal static extern IntPtr Aes128Cfb8();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_EncryptUpdate")]
        internal static extern int EncryptUpdate(IntPtr handle, IntPtr output, IntPtr outLen, IntPtr input, int inLen);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_DecryptUpdate")]
        internal static extern int DecryptUpdate(IntPtr handle, IntPtr output, IntPtr outLen, IntPtr input, int inLen);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EVP_CIPHER_CTX_free")]
        internal static extern void DeleteCipher(IntPtr handle);
    }
}