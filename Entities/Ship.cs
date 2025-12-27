using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidsClone.Core;
using AsteroidsClone.Systems;
using AsteroidsClone.Rendering.Particles;

namespace AsteroidsClone.Entities;

public class Ship : GameObject
{
    // Configuration
    public float RotationSpeed { get; set; } = 5f;      // Radians/second
    public float ThrustForce { get; set; } = 300f;      // Pixels/second²
    public float MaxSpeed { get; set; } = 600f;         // Pixels/second (higher for classic feel)
    
    // Systems
    public PowerSystem Power { get; }
    public BuffSystem Buffs { get; }
    
    // State
    public bool IsThrusting { get; private set; }
    public bool IsShielded => Power.IsForceFieldActive;
    public bool IsInvulnerable { get; set; }
    public float InvulnerabilityTimer { get; set; }
    
    private float _fireCooldown;
    private static Random _random = new Random();
    
    public Ship(PowerSystem powerSystem = null)
    {
        Power = powerSystem ?? new PowerSystem();
        Buffs = new BuffSystem();
        Radius = 15f;
        Position = new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2);
        IsActive = true;
        IsInvulnerable = false;
        InvulnerabilityTimer = 0;
    }
    
    public override void Update(float deltaTime)
    {
        Power.Update(deltaTime);
        Buffs.Update(deltaTime);
        
        if (InvulnerabilityTimer > 0)
        {
            InvulnerabilityTimer -= deltaTime;
            if (InvulnerabilityTimer <= 0)
                IsInvulnerable = false;
        }
        
        _fireCooldown = Math.Max(0, _fireCooldown - deltaTime);
        
        HandleInput(deltaTime);
        
        // No drag - classic Asteroids physics! Momentum is conserved in space.

        // Apply speed multiplier from buffs
        float speedMultiplier = Buffs.GetSpeedMultiplier();
        float effectiveMaxSpeed = MaxSpeed * speedMultiplier;
        
        // Clamp speed
        if (Velocity.Length() > effectiveMaxSpeed)
        {
            Velocity = Vector2.Normalize(Velocity) * effectiveMaxSpeed;
        }
        
        base.Update(deltaTime);
    }
    
    private void HandleInput(float deltaTime)
    {
        // Rotation (free, no power cost)
        if (InputManager.IsHeld(Keys.Left) || InputManager.IsHeld(Keys.A))
            Rotation -= RotationSpeed * deltaTime;
        if (InputManager.IsHeld(Keys.Right) || InputManager.IsHeld(Keys.D))
            Rotation += RotationSpeed * deltaTime;
        
        // Thrust (power-dependent)
        IsThrusting = false;
        if (InputManager.IsHeld(Keys.Up) || InputManager.IsHeld(Keys.W))
        {
            float effectiveness = Power.ApplyThrustDrain(deltaTime);
            Vector2 thrustVector = new Vector2(
                MathF.Cos(Rotation),
                MathF.Sin(Rotation)
            ) * ThrustForce * effectiveness * deltaTime;
            
            Velocity += thrustVector;
            IsThrusting = effectiveness > 0;

            if (IsThrusting)
            {
                Vector2 thrustPos = Position - new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation)) * Radius;
                ParticleSystem.Instance?.CreateThrustEffect(thrustPos, Rotation, effectiveness);
            }
        }
        
        // Fire
        if (InputManager.IsPressed(Keys.Space) && _fireCooldown <= 0)
        {
            Fire();
        }
        
        // Force Field
        if (InputManager.IsPressed(Keys.LeftShift))
        {
            ActivateForceField();
        }
        
        // Hyperspace
        if (InputManager.IsPressed(Keys.H))
        {
            Hyperspace();
        }
    }
    
    public void Fire()
    {
        if (!Power.CanFire()) 
        {
            // AudioManager.Play(SoundEffect.PowerEmpty);
            return;
        }
        
        if (Power.TryFire())
        {
            float cooldown = Buffs.GetFireCooldownMultiplier() * 0.25f;
            _fireCooldown = cooldown;
            
            int bulletCount = Buffs.GetBulletCount();
            float spreadAngle = 0.2f;
            
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = Rotation;
                if (bulletCount > 1)
                {
                    angle += (i - (bulletCount - 1) / 2f) * spreadAngle;
                }
                
                Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
                Vector2 bulletPos = Position + direction * Radius;
                
                var bullet = new Bullet(bulletPos, direction, true);
                if (Buffs.HasPiercing()) bullet.IsPiercing = true;
                
                GameState.AddEntity(bullet);
            }

            Audio.AudioManager.Play(Audio.SoundEffect.Fire);
        }
    }
    
    public void ActivateForceField()
    {
        if (Power.TryActivateForceField())
        {
            // AudioManager.Play(SoundEffect.ShieldActivate);
        }
        else
        {
            // AudioManager.Play(SoundEffect.PowerEmpty);
        }
    }
    
    public void ActivateForceFieldFree()
    {
        // From power-up, no power cost
        Power.ActivateForceFieldFree();
        // AudioManager.Play(SoundEffect.ShieldActivate);
    }
    
    public void Hyperspace()
    {
        if (!Power.CanHyperspace())
        {
            // AudioManager.Play(SoundEffect.PowerEmpty);
            return;
        }
        
        if (Power.TryHyperspace())
        {
            Vector2 oldPosition = Position;
            
            // Random new position
            Position = new Vector2(
                _random.Next(50, GameState.ScreenWidth - 50),
                _random.Next(50, GameState.ScreenHeight - 50)
            );
            
            // Hyperspace particle effect
            ParticleSystem.Instance?.CreateHyperspaceEffect(oldPosition, Position);
            
            // Brief invulnerability after jump
            IsInvulnerable = true;
            InvulnerabilityTimer = 0.5f;
        }
    }
    
    public bool TakeDamage()
    {
        if (IsInvulnerable) return false;
        
        if (IsShielded)
        {
            ParticleSystem.Instance?.CreateShieldHitEffect(Position, Vector2.Zero);
            return false;
        }
        
        return true; // Ship destroyed
    }
    
    public void Respawn(Vector2 position)
    {
        Position = position;
        Velocity = Vector2.Zero;
        Rotation = 0;
        IsActive = true;
        IsInvulnerable = true;
        InvulnerabilityTimer = 2.0f;
        Power.CurrentPower = Power.MaxPower; // Reset power on respawn
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering in Phase 8
        // For now, draw a simple triangle
        if (IsInvulnerable && (int)(InvulnerabilityTimer * 10) % 2 == 0)
        {
            return; // Flash effect
        }
        
        // Simple placeholder rendering
        // VectorRenderer.DrawShip(Position, Rotation, IsThrusting, IsShielded);
    }
}
