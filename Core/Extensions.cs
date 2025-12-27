using System;
using Microsoft.Xna.Framework;

namespace AsteroidsClone.Core;

public static class Extensions
{
    private static readonly Random _random = new Random();
    
    public static float NextFloat(this Random random, float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
    
    public static Vector2 Rotate(this Vector2 vector, float angle)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(
            vector.X * cos - vector.Y * sin,
            vector.X * sin + vector.Y * cos
        );
    }
    
    public static float ToRadians(this float degrees)
    {
        return degrees * MathF.PI / 180f;
    }
    
    public static float ToDegrees(this float radians)
    {
        return radians * 180f / MathF.PI;
    }
}


