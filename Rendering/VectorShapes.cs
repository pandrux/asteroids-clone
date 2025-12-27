using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace AsteroidsClone.Rendering;

public static class VectorShapes
{
    // Ship shape (triangle pointing RIGHT - angle 0 = right in standard trig)
    public static List<Vector2> ShipShape => new List<Vector2>
    {
        new Vector2(15, 0),    // Nose (right)
        new Vector2(-10, -10), // Back top
        new Vector2(-10, 10)   // Back bottom
    };
    
    // Large asteroid shape (irregular polygon)
    public static List<Vector2> LargeAsteroidShape => new List<Vector2>
    {
        new Vector2(0, -40),
        new Vector2(15, -30),
        new Vector2(30, -15),
        new Vector2(35, 5),
        new Vector2(25, 25),
        new Vector2(5, 35),
        new Vector2(-15, 30),
        new Vector2(-30, 15),
        new Vector2(-35, -5),
        new Vector2(-25, -25),
        new Vector2(-10, -35)
    };
    
    // Medium asteroid shape
    public static List<Vector2> MediumAsteroidShape => new List<Vector2>
    {
        new Vector2(0, -20),
        new Vector2(8, -15),
        new Vector2(15, -8),
        new Vector2(18, 3),
        new Vector2(13, 13),
        new Vector2(3, 18),
        new Vector2(-8, 15),
        new Vector2(-15, 8),
        new Vector2(-18, -3),
        new Vector2(-13, -13),
        new Vector2(-5, -18)
    };
    
    // Small asteroid shape
    public static List<Vector2> SmallAsteroidShape => new List<Vector2>
    {
        new Vector2(0, -10),
        new Vector2(4, -8),
        new Vector2(8, -4),
        new Vector2(9, 2),
        new Vector2(7, 7),
        new Vector2(2, 9),
        new Vector2(-4, 8),
        new Vector2(-8, 4),
        new Vector2(-9, -2),
        new Vector2(-7, -7),
        new Vector2(-3, -9)
    };
    
    // UFO shape (saucer)
    public static List<Vector2> UFOShape => new List<Vector2>
    {
        new Vector2(-25, 0),
        new Vector2(-20, -8),
        new Vector2(-10, -12),
        new Vector2(10, -12),
        new Vector2(20, -8),
        new Vector2(25, 0),
        new Vector2(20, 8),
        new Vector2(10, 12),
        new Vector2(-10, 12),
        new Vector2(-20, 8)
    };
    
    // Boss UFO shape (larger saucer)
    public static List<Vector2> BossUFOShape => new List<Vector2>
    {
        new Vector2(-50, 0),
        new Vector2(-40, -15),
        new Vector2(-20, -25),
        new Vector2(20, -25),
        new Vector2(40, -15),
        new Vector2(50, 0),
        new Vector2(40, 15),
        new Vector2(20, 25),
        new Vector2(-20, 25),
        new Vector2(-40, 15)
    };
    
    // Bullet shape (small line pointing right - direction of travel)
    public static List<Vector2> BulletShape => new List<Vector2>
    {
        new Vector2(-3, 0),
        new Vector2(3, 0)
    };
    
    // Power-up shape (diamond)
    public static List<Vector2> PowerUpShape => new List<Vector2>
    {
        new Vector2(0, -15),
        new Vector2(15, 0),
        new Vector2(0, 15),
        new Vector2(-15, 0)
    };

    // Companion drone shape (diamond with trailing fins, pointing right)
    public static List<Vector2> CompanionDroneShape => new List<Vector2>
    {
        new Vector2(8, 0),   // Nose
        new Vector2(0, -5),  // Top
        new Vector2(-6, 0),  // Back
        new Vector2(0, 5)    // Bottom
    };

    // Companion drone fins (separate for multi-line drawing)
    public static List<Vector2> CompanionDroneFins => new List<Vector2>
    {
        new Vector2(-6, 0),   // Fin start
        new Vector2(-10, -3), // Left fin tip
        new Vector2(-6, 0),   // Return
        new Vector2(-10, 3)   // Right fin tip
    };
}


