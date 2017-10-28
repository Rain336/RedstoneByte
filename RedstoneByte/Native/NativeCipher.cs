using System;

namespace RedstoneByte.Native
{
    public sealed class NativeCipher : IDisposable
    {
        public readonly IntPtr Handle;
        public readonly bool Encrypting;
        private bool _disposed = false;

        public NativeCipher(bool encrypting)
        {
            Encrypting = encrypting;
            Handle = OpenSSL.NewCipher();
            if (Handle == IntPtr.Zero)
                throw new OpenSslException(Handle.ToInt32());
        }

        ~NativeCipher()
        {
            Dispose();
        }

        public unsafe void Init(byte[] key, byte[] iv)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            int result;
            fixed (void* keyPtr = &key[0])
            fixed (void* ivPtr = &iv[0])
            {
                if (Encrypting)
                {
                    result = OpenSSL.InitEncryptingCipher(Handle, OpenSSL.Aes128Cfb8(),
                        IntPtr.Zero, (IntPtr) keyPtr, (IntPtr) ivPtr);
                }
                else
                {
                    result = OpenSSL.InitDecryptingCipher(Handle, OpenSSL.Aes128Cfb8(),
                        IntPtr.Zero, (IntPtr) keyPtr, (IntPtr) ivPtr);
                }
            }
            if (result != 1)
                throw new OpenSslException(result);
        }

        public byte[] Process(byte[] array)
            => Process(array, 0, array.Length);

        public byte[] Process(byte[] array, int offset, int length)
        {
            var ret = new byte[length];
            Process(array, offset, length, ret, 0);
            return ret;
        }

        public int Process(byte[] input, byte[] output)
            => Process(input, 0, input.Length, output, 0);

        public unsafe int Process(byte[] input, int inOffset, int inLength, byte[] output, int outOffset)
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().FullName);

            fixed (void* inPtr = &input[inOffset])
            fixed (void* outPtr = &output[outOffset])
            {
                var i = 0;
                var result = Encrypting
                    ? OpenSSL.EncryptUpdate(Handle, (IntPtr) outPtr, (IntPtr) (&i), (IntPtr) inPtr, inLength)
                    : OpenSSL.DecryptUpdate(Handle, (IntPtr) outPtr, (IntPtr) (&i), (IntPtr) inPtr, inLength);
                if (result != 1)
                    throw new OpenSslException(result);
                return i;
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            OpenSSL.DeleteCipher(Handle);
            _disposed = true;
        }
    }

    public class OpenSslException : Exception
    {
        public OpenSslException(int e) : base("Error Code: 0x" + e.ToString("X"))
        {
        }
    }
}