using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RedstoneByte.Test
{
    [TestClass]
    public static class VarIntReadSpeed
    {
        public static int Index = 0;
        public static readonly Random Random = new Random();

        public static byte ReadByte(this List<byte> list)
        {
            var b = list[Index];
            Index++;
            return b;
        }

        public static int ReadVarIntUnrolled(this List<byte> buffer)
        {
            int value;
            do
            {
                var b = buffer.ReadByte(); value = (b & 0x7F); if ((b & 0x80) == 0) break;
                b = buffer.ReadByte(); value |= (b & 0x7F) << 7; if ((b & 0x80) == 0) break;
                b = buffer.ReadByte(); value |= (b & 0x7F) << 14; if ((b & 0x80) == 0) break;
                b = buffer.ReadByte(); value |= (b & 0x7F) << 21; if ((b & 0x80) == 0) break;
                b = buffer.ReadByte(); value |= (b & 0x7F) << 28; if ((b & 0x80) == 0) break;
                throw new FormatException("VarInt may not be longer than 5 bytes.");
            } while (false);
            return value;
        }


        public static void WriteVarInt(this List<byte> buffer, int value)
        {
            while ((value & 0xFFFFFF80) != 0)
            {
                buffer.Add((byte)((value & 0x7F) | 0x80));
                value >>= 7;
            }
            buffer.Add((byte)value);
        }

        public static int ReadVarInt(this List<byte> buffer)
        {
            var value = 0;
            var size = 0;
            while (true)
            {
                var b = buffer.ReadByte();
                value |= (b & 0x7F) << size++ * 7;
                if (size > 5)
                    throw new FormatException("VarInt may not be longer than 5 bytes.");
                if ((b & 0x80) != 128)
                    break;
            }
            return value;
        }

        [TestMethod]
        public static void SpeedTest()
        {
            var input = new List<int>();
            for (var i = 0; i < 1000; i++)
            {
                input.Add(Random.Next());
            }

            var encoded = new List<byte>();
            foreach (var i in input)
            {
                encoded.WriteVarInt(i);
            }

            var watch = Stopwatch.StartNew();
            foreach (var i in input)
            {
                Assert.AreEqual(encoded.ReadVarIntUnrolled(), i);
            }
            watch.Stop();

            Index = 0;
            Console.WriteLine("Unrolled: " + watch.ElapsedTicks + "t");
            watch.Reset();

            watch.Start();
            foreach (var i in input)
            {
                Assert.AreEqual(encoded.ReadVarInt(), i);
            }
            watch.Stop();

            Console.WriteLine("Rolled: " + watch.ElapsedTicks + "t");
        }
    }
}