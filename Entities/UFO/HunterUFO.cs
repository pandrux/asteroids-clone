using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class HunterUFO : UFOBase
{
    private float _huntSpeed = 80f;
    
    public HunterUFO()
    {
        PointValue = 300;
        Health = 2;
        MaxHealth = 2;
        Radius = 25f;
        FireCooldown = 1.5f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        // Hunt the player
        if (GameState.Player != null && GameState.Player.IsActive)
        {
            Vector2 direction = Vector2.Normalize(GameState.Player.Position - Position);
            Velocity = direction * _huntSpeed;
        }
        
        // Fire at player
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.4f, 0.4f);
        }
    }
}
