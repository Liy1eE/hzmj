using System;
using System.Collections.Generic;

namespace test
{
    public class Test
    {
        private static void Main()
        {
            Over4Cards();
        }

        private static void Over4Cards()
        {
            var results = new List<int[]>();
            for (int x = 5; x < 9; x++)
            {
                for (int a = 0; a < 2; a++)
                {
                    for (int b = 0; b < 3; b++)
                    {
                        int c = x - 2 * a - 3 * b;
                        if (c >= 0 && c <= 4 && b + c <= 4)
                        {
                            results.Add(new[] { x, a, b, c });
                            Console.WriteLine(x + "=" + a + "*2+" + b + "*3+" + c);
                        }
                    }
                }
            }
            Console.ReadKey();
        }
    }
}