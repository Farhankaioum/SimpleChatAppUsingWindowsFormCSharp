using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection.Console
{
    public class Test
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Msg()
        {
            System.Console.WriteLine("Greeting from Test class");
        }
    }

    public class Test1
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Test test = new Test
            {
                Id = 1,
                Name = "H"
            };

            PrintProperties(test);
            System.Console.WriteLine();

            Test1 test1 = new Test1
            {
                Id = 12,
                Name = "abc"
            };

            PrintProperties(test);

        }

        public static void PrintProperties<T>(T obj) 
        {
            Type type = obj.GetType();

             //IList<PropertyInfo> props = new List<PropertyInfo>(type.GetProperties());
             IList<PropertyInfo> propertyInfos = type.GetProperties();

            


                foreach (var prop in propertyInfos)
                {
                    object propValue = prop.GetValue(obj, null);

                System.Console.WriteLine(propValue);

                }

            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                method.Invoke(type, null);
            }

        }

    }
}

