using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Entities;

namespace AsteroidsClone.Entities.UFO;

public class BomberUFO : UFOBase
{
    private float _mineDropCooldown;
    private const float MineDropInterval = 3.0f;
    
    public BomberUFO()
    {
        PointValue = 250;
        Health = 2;
        MaxHealth = 2;
        Radius = 25f;
        FireCooldown = 2.0f;
        _mineDropCooldown = MineDropInterval;
        
        // Moves slowly
        Vector2 direction = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        direction.Normalize();
        Velocity = direction * 40f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        _mineDropCooldown -= deltaTime;
        
        // Drop mines
        if (_mineDropCooldown <= 0)
        {
            var mine = new Mine(Position);
            GameState.AddEntity(mine);
            _mineDropCooldown = MineDropInterval + _random.NextFloat(-0.5f, 0.5f);
        }
        
        // Fire occasionally
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.5f, 0.5f);
        }
    }
}
