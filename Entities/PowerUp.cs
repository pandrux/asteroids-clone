using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.Entities;

public enum PowerUpType
{
    MultiShot,
    Piercing,
    RapidFire,
    DamageBoost,
    ShieldBoost,
    HealthRegen,
    Invulnerability,
    PowerRecharge,
    SpeedBoost,
    ScoreMultiplier,
    ExtraLife
}

public class PowerUp : GameObject
{
    public PowerUpType Type { get; private set; }
    private float _lifetime;
    private const float MaxLifetime = 10.0f;
    private float _pulseTimer;
    
    public PowerUp(PowerUpType type, Vector2 position)
    {
        Type = type;
        Position = position;
        Velocity = Vector2.Zero;
        Radius = 15f;
        _lifetime = MaxLifetime;
        _pulseTimer = 0;
    }
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _lifetime -= deltaTime;
        _pulseTimer += deltaTime * 3f;
        if (_lifetime <= 0)
        {
            IsActive = false;
        }
    }
    
    public void Apply(Ship ship)
    {
        switch (Type)
        {
            case PowerUpType.MultiShot:
                ship.Buffs.AddBuff("MultiShot", 15f);
                break;
            case PowerUpType.Piercing:
                ship.Buffs.AddBuff("Piercing", 20f);
                break;
            case PowerUpType.RapidFire:
                ship.Buffs.AddBuff("RapidFire", 15f);
                break;
            case PowerUpType.DamageBoost:
                ship.Buffs.AddBuff("DamageBoost", 20f);
                break;
            case PowerUpType.ShieldBoost:
                ship.ActivateForceFieldFree();
                break;
            case PowerUpType.HealthRegen:
                // Restore a life (if not at max)
                if (GameState.Lives < 5)
                {
                    GameState.Lives++;
                }
                break;
            case PowerUpType.Invulnerability:
                ship.Buffs.AddBuff("Invulnerability", 5f);
                ship.IsInvulnerable = true;
                ship.InvulnerabilityTimer = 5f;
                break;
            case PowerUpType.PowerRecharge:
                ship.Power.AddPower(50f);
                break;
            case PowerUpType.SpeedBoost:
                ship.Buffs.AddBuff("SpeedBoost", 15f);
                break;
            case PowerUpType.ScoreMultiplier:
                ship.Buffs.AddBuff("ScoreMultiplier", 30f);
                GameState.ScoreMultiplier = 2.0f;
                break;
            case PowerUpType.ExtraLife:
                GameState.Lives++;
                break;
        }
        
        // AudioManager.Play(SoundEffect.PowerUpCollect);
        IsActive = false;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering
        // Visual indicator based on type
    }
}
