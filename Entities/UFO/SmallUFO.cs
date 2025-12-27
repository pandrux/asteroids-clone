using Microsoft.Xna.Framework;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities.UFO;

public class SmallUFO : UFOBase
{
    public SmallUFO()
    {
        PointValue = 1000;
        Health = 1;
        MaxHealth = 1;
        Radius = 20f;
        FireCooldown = 1.0f;
        
        // Small UFO moves faster and more erratically
        Vector2 direction = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        direction.Normalize();
        Velocity = direction * 150f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        // Small UFO fires more frequently
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.3f, 0.3f);
        }
    }
}
