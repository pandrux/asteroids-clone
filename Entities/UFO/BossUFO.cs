using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class BossUFO : UFOBase
{
    public int Phase { get; set; } = 1;
    private float _phaseChangeTimer;
    private Vector2 _movementDirection;
    
    public BossUFO()
    {
        PointValue = 5000;
        Health = 20;
        MaxHealth = 20;
        Radius = 50f;
        FireCooldown = 0.8f;
        _phaseChangeTimer = 0;
        _movementDirection = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        _movementDirection.Normalize();
        Velocity = _movementDirection * 40f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        _phaseChangeTimer += deltaTime;
        
        // Change movement pattern based on phase
        if (Phase == 1)
        {
            // Slow, predictable movement
            if (_phaseChangeTimer > 3f)
            {
                _movementDirection = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
                _movementDirection.Normalize();
                Velocity = _movementDirection * 40f;
                _phaseChangeTimer = 0;
            }
        }
        else if (Phase == 2)
        {
            // Faster, more aggressive
            if (GameState.Player != null && GameState.Player.IsActive)
            {
                Vector2 direction = Vector2.Normalize(GameState.Player.Position - Position);
                Velocity = direction * 60f;
            }
            FireCooldown = 0.5f; // Fire faster
        }
        
        // Fire frequently
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown;
        }
    }
    
    public override bool TakeDamage()
    {
        Health--;
        if (Health <= MaxHealth * 0.5f && Phase == 1)
        {
            Phase = 2;
        }
        return Health <= 0;
    }
}
