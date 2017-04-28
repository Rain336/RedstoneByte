using System;
using System.IO;

namespace RedstoneByte.Test
{
    public class TestFileFixture : IDisposable
    {
        private readonly StreamWriter _stream;

        public TestFileFixture()
        {
            _stream = File.CreateText("Test.log");
        }

        public void WriteLine<T>(string str, T cls)
            => WriteLine(str, cls?.GetType());

        public void WriteLine(string str, Type cls)
        {
            if(cls == null)
                throw new ArgumentNullException(nameof(cls));
            if(str == null)
                throw new ArgumentNullException(nameof(str));

            _stream.WriteLine(string.Format("[{0}] {1}", cls, str));
        }

        public void Dispose()
        {
            _stream.Flush();
            _stream.Dispose();
        }
    }
}