using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class LargeUFO : UFOBase
{
    private float _directionChangeTimer;
    private Vector2 _currentDirection;
    
    public LargeUFO()
    {
        PointValue = 200;
        Health = 3;
        MaxHealth = 3;
        Radius = 30f;
        FireCooldown = 2.0f;
        _directionChangeTimer = 0;
        _currentDirection = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        _currentDirection.Normalize();
        Velocity = _currentDirection * 50f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        _directionChangeTimer -= deltaTime;
        
        if (_directionChangeTimer <= 0)
        {
            _currentDirection = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
            _currentDirection.Normalize();
            Velocity = _currentDirection * 50f;
            _directionChangeTimer = _random.NextFloat(2f, 4f);
        }
        
        // Fire occasionally
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.5f, 0.5f);
        }
    }
}
