using UnityEngine;

namespace AsteroidFoundry
{
  public static class VectorExtensions
    {
        public static Vector2 Direction(float angle)
        {
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            return new Vector2(cos, sin);
        }

        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            var x = vector.x * cos - vector.y * sin;
            var y = vector.x * sin + vector.y * cos;
            return new Vector2(x, y);
        }
    }
}
