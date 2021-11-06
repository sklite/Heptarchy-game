using System;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    class MathCalculator
    {
        const float Epsilon = 0.001f;
        public const float MinNodeDistance = 0.8f;

        public static float CalcDistance(float x1, float x2, float y1, float y2)
        {
            return (float)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static double ToTrigonoimetricAngleDeg(double rad)
        {
            var oneRadRatio = 57.2958;
            var convertedToDegree = rad * oneRadRatio;
            return convertedToDegree < 0 ? 360 + convertedToDegree : convertedToDegree;
        }

        public static bool EqualVectors(Vector3 v1, Vector3 v2, float epsilon = Epsilon)
        {
            return EqualsFloat(v1.x, v2.x, epsilon) && EqualsFloat(v1.y, v2.y, epsilon) && EqualsFloat(v1.z, v2.z, epsilon);
        }

        public static bool EqualsFloat(float f1, float f2, float epsilon = Epsilon)
        {
            var abs = Math.Abs(f2 - f1);
            if (abs > epsilon)
                return false;
            return true;
        }

        public static Vector3 Calc2DSpeed(Vector3 source, Vector3 destination, float speed)
        {
            float katetX = Mathf.Abs(source.x - destination.x);
            float katetY = Mathf.Abs(source.y - destination.y);
            float hypotenuza = (float)Mathf.Sqrt(katetX * katetX + katetY * katetY);

            var dx = speed * (katetX / hypotenuza);
            var dy = speed * (katetY / hypotenuza);

            if (source.y > destination.y)
                dy *= -1;

            if (source.x > destination.x)
                dx *= -1;

            return new Vector3(dx, dy);
        }
    }
}
