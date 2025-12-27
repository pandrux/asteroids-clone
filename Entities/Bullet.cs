using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Entities;

public class Bullet : GameObject
{
    public bool IsPlayerBullet { get; set; }
    public bool IsPiercing { get; set; }
    private float _lifetime;
    private const float MaxLifetime = 3.0f;
    
    public Bullet(Vector2 position, Vector2 direction, bool isPlayerBullet)
    {
        Position = position;
        Velocity = direction * 600f; // Bullet speed
        IsPlayerBullet = isPlayerBullet;
        Radius = 3f;
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


