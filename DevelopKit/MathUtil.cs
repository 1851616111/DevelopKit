using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevelopKit
{
    public static class MathUtil
    {
        public static int MaxSpan(int a, int b, int c)
        {
            int max = a > b ? a : b;
            max = max > c ? max : c;

            int min = a < b ? a : b;
            min = min < c ? min : c;

            return max - min;
        }

        public static int Max(int a, int b, int c)
        {
            int max = a > b ? a : b;
            return max > c ? max : c;
        }
        public static int Min(int a, int b, int c)
        {
            int min = a < b ? a : b;
            return min < c ? min : c;
        }
    }
}
