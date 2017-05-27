using System;
using System.Linq;
using System.Reflection;

namespace RedstoneByte.Test
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Running Tests!");
            foreach (var info in Assembly.GetEntryAssembly().GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(t => t.GetCustomAttribute<TestAttribute>() != null))
            {
                Console.WriteLine("Strting Test '{0}", info.Name);
                try
                {
                    info.Invoke(null, new object[0]);
                    Console.WriteLine("Test Succeeded");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Test Errored!");
                    Console.WriteLine(e);
                }
            }
        }
    }
}