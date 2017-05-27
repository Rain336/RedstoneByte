using System;
using System.Collections.Generic;
using System.Linq;

namespace RedstoneByte.Test
{
    public static class Assert
    {
        public static void Equal(object test, object expected)
        {
            if(!expected.Equals(test))
                throw new Exception("Assertion Failed!");
        }
        
        public static void Equal<T>(IEnumerable<T> test, IEnumerable<T> expected)
        {
            if(!expected.SequenceEqual(test))
                throw new Exception("Assertion Failed!");
        }

        public static void Null(object test)
        {
            if(test != null)
                throw new Exception("Assertion Failed!");
        }
        
        public static void NotNull(object test)
        {
            if(test == null)
                throw new Exception("Assertion Failed!");
        }

        public static void IsType<T>(object test)
        {
            NotNull(test);
            if(test.GetType() != typeof(T))
                throw new Exception("Assertion Failed!");                
        }
    }
}