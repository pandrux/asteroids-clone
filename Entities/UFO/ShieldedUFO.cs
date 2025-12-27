using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class ShieldedUFO : UFOBase
{
    public bool IsShielded { get; set; } = true;
    private float _shieldRegenTimer;
    private const float ShieldRegenTime = 5.0f;
    
    public ShieldedUFO()
    {
        PointValue = 400;
        Health = 3;
        MaxHealth = 3;
        Radius = 25f;
        FireCooldown = 1.8f;
        _shieldRegenTimer = ShieldRegenTime;
        
        Vector2 direction = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        direction.Normalize();
        Velocity = direction * 60f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        // Regenerate shield after time
        if (!IsShielded)
        {
            _shieldRegenTimer -= deltaTime;
            if (_shieldRegenTimer <= 0)
            {
                IsShielded = true;
                _shieldRegenTimer = ShieldRegenTime;
            }
        }
        
        // Fire at player
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.4f, 0.4f);
        }
    }
    
    public override bool TakeDamage()
    {
        if (IsShielded)
        {
            IsShielded = false;
            _shieldRegenTimer = ShieldRegenTime;
            return false;
        }
        return base.TakeDamage();
    }
}
