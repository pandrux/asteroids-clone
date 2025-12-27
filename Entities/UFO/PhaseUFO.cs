using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class PhaseUFO : UFOBase
{
    private float _phaseCooldown;
    private const float PhaseInterval = 4.0f;
    private bool _isPhasing;
    
    public PhaseUFO()
    {
        PointValue = 350;
        Health = 2;
        MaxHealth = 2;
        Radius = 25f;
        FireCooldown = 1.5f;
        _phaseCooldown = PhaseInterval;
        _isPhasing = false;
        
        Vector2 direction = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        direction.Normalize();
        Velocity = direction * 70f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        _phaseCooldown -= deltaTime;
        
        // Phase/teleport
        if (_phaseCooldown <= 0 && !_isPhasing)
        {
            Teleport();
            _phaseCooldown = PhaseInterval + _random.NextFloat(-1f, 1f);
        }
        
        // Fire at player
        if (_fireTimer <= 0 && !_isPhasing)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.4f, 0.4f);
        }
    }
    
    private void Teleport()
    {
        _isPhasing = true;
        // Teleport to random position
        Position = new Vector2(
            _random.Next(100, GameState.ScreenWidth - 100),
            _random.Next(100, GameState.ScreenHeight - 100)
        );
        _isPhasing = false;
    }
}
