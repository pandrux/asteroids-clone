using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Entities;

public class Drone : GameObject
{
    private float _lifetime;
    private const float MaxLifetime = 15.0f;
    
    public Drone(Vector2 position, Vector2 velocity)
    {
        Position = position;
        Velocity = velocity;
        Radius = 8f;
        _lifetime = MaxLifetime;
    }
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _lifetime -= deltaTime;
        if (_lifetime <= 0)
        {
            IsActive = false;
        }
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering
    }
}


