using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Entities;

public class Mine : GameObject
{
    private float _armTime;
    private const float ArmTime = 1.0f;
    private bool _isArmed;
    
    public Mine(Vector2 position)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Radius = 20f;
        _armTime = ArmTime;
        _isArmed = false;
    }
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _armTime -= deltaTime;
        if (_armTime <= 0 && !_isArmed)
        {
            _isArmed = true;
        }
    }
    
    public void Explode()
    {
        IsActive = false;
        // ParticleSystem.CreateExplosion(Position, Radius * 2, Color.Orange);
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering
    }
}


