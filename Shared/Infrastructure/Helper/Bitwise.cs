using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace myuzbekistan.Shared;

public static class Bitwise
{
    public static bool IsSetKthBit(int n, int k)
    {
        return ((1 << k) & n) != 0;
    }

    public static int SetKthBit(int n, int k)
    {
        return (n | (1 << k));
    }

    public static int UnSetKthBit(int n, int k)
    {
        return (n & ~(1 << k));
    }

    public static bool IsMenu(int n)
    {
        return IsSetKthBit(n, (int)ContentFields.Menu);
    }
    public static bool Is(int n,ContentFields contentFields)
    {
        return IsSetKthBit(n, (int)contentFields);
    }
}
