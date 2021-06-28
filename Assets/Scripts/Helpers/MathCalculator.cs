using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    class MathCalculator
    {
        const float Epsilon = 0.001f;

        public static float CalcDistance(float x1, float x2, float y1, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static bool EqualVectors(Vector3 v1, Vector3 v2)
        {
            var result = v2 - v1;

            return EqualsFloat(v1.x, v2.x) && EqualsFloat(v1.y, v2.y) && EqualsFloat(v1.z, v2.z);
        }

        public static bool EqualsFloat(float f1, float f2)
        {
            var abs = Math.Abs(f2 - f1);
            if (abs > Epsilon)
                return false;
            return true;
        }
    }
}
