using DotNetty.Buffers;
using RedstoneByte.Networking;
using Xunit;

namespace RedstoneByte.Test
{
    [Collection("TestFile")]
    public class VarIntTest
    {
        private readonly TestFileFixture _file;

        public VarIntTest(TestFileFixture file)
        {
            _file = file;
        }

        [Fact]
        public void OneByte_Read()
        {
            var input = Unpooled.WrappedBuffer(new byte[] {0xFF});
            var result = input.ReadVarInt();
            _file.WriteLine(0xFF + " == " + result, this);
            Assert.Equal(0xFF, result);
        }

        [Fact]
        public void OneByte_Write()
        {
            var input = Unpooled.Buffer(1);
            input.WriteVarInt(0xFF);
            var result = input.Array[0];
            _file.WriteLine(0xFF + " == " + result, this);
            Assert.Equal(0xFF, result);
        }
    }
}