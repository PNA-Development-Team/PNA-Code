using System;
using System.Linq;
using System.Collections.Generic;
using MathematicalTool;

namespace Test
{
    public class A
    {
        public string a = string.Empty;
        public string b = string.Empty;
        public A(string a,string b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Equals(object obj)
        {
            return this.a == (obj as A).a;
        }

        public override int GetHashCode()
        {
            return 1;
        }

        public static bool operator==(A num1,A num2)
        {
            return num1.Equals(num2);
        }
        public static bool operator!=(A num1,A num2)
        {
            return !(num1 == num2);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            HashSet<A> set = new HashSet<A>();
            set.Add(new A("1", "2"));
            set.Add(new A("2", "2"));
            set.Add(new A("3", "2"));
            set.Add(new A("1", "4"));
            if (!(set.Contains(new A("4", "2"))))
                set.Add(new A("4", "2"));

            foreach(A item in set)
            {
                Console.WriteLine("a = " + item.a + "," + "b = " + item.b);
            }

            string a = "12";
            string b = "12";
            Console.WriteLine(a.GetHashCode());
            Console.WriteLine(b.GetHashCode());

            Console.ReadKey();
        }
    }
}
