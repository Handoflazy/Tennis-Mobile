using UnityEngine;

namespace Utilities.Extensions
{
    public static class MyMath
    {
        public static int PowInt(int x, int y) {
            return (int) Mathf.Pow(x, y);
        }
    }
}