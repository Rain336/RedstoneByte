using System;
using System.Collections.Generic;
using System.Text;
using RedstoneByte.Native;

namespace RedstoneByte.Test
{
    public static class NativeCipherTest
    {
        [Test]
        public static void HelloWorld()
        {
            var result = Encoding.UTF8.GetBytes("Hello World!");
            var key = new byte[]
            {
                0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF
            };
            var iv = new byte[16];
            HexDump(result);

            byte[] encrypted;
            using (var cipher = new NativeCipher(true))
            {
                cipher.Init(key, iv);
                encrypted = cipher.Process(result);
            }
            HexDump(encrypted);

            byte[] decrypted;
            using (var cipher = new NativeCipher(false))
            {
                cipher.Init(key, iv);
                decrypted = cipher.Process(encrypted);
            }
            HexDump(decrypted);

            Assert.Equal(result, decrypted);
        }

        private static void HexDump(IEnumerable<byte> encrypted)
        {
            Console.WriteLine("----------------------------------------------");
            foreach (var t in encrypted)
            {
                Console.Write("0x{0:X} ", t);
            }
            Console.WriteLine();
            Console.WriteLine("----------------------------------------------");
        }
    }
}