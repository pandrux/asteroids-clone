using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Entities;

public enum AsteroidSize
{
    Large,
    Medium,
    Small
}

public class Asteroid : GameObject
{
    public AsteroidSize Size { get; private set; }
    public int PointValue { get; private set; }
    
    public Asteroid(AsteroidSize size, Vector2 position, Vector2 velocity)
    {
        Size = size;
        Position = position;
        Velocity = velocity;
        
        switch (size)
        {
            case AsteroidSize.Large:
                Radius = 40f;
                PointValue = 20;
                break;
            case AsteroidSize.Medium:
                Radius = 20f;
                PointValue = 50;
                break;
            case AsteroidSize.Small:
                Radius = 10f;
                PointValue = 100;
                break;
        }
    }
    
    public List<Asteroid> Split()
    {
        var children = new List<Asteroid>();
        
        if (Size == AsteroidSize.Large)
        {
            // Split into 2 medium asteroids
            for (int i = 0; i < 2; i++)
            {
                float angle = (float)(System.Math.PI * 2 * i / 2);
                Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                children.Add(new Asteroid(AsteroidSize.Medium, Position, direction * 100f));
            }
        }
        else if (Size == AsteroidSize.Medium)
        {
            // Split into 2 small asteroids
            for (int i = 0; i < 2; i++)
            {
                float angle = (float)(System.Math.PI * 2 * i / 2);
                Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                children.Add(new Asteroid(AsteroidSize.Small, Position, direction * 150f));
            }
        }
        // Small asteroids don't split
        
        return children;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering
    }
}

