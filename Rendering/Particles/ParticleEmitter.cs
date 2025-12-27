using System;
using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using static AsteroidsClone.Core.Extensions;

namespace AsteroidsClone.Rendering.Particles;

public static class ParticleEmitter
{
    private static Random _random = new Random();
    
    public static void EmitExplosion(Vector2 position, float radius, Color color, int count, ParticleSystem particleSystem)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = _random.NextFloat(0, (float)(Math.PI * 2));
            float speed = ParticleTemplate.Explosion.Speed + _random.NextFloat(-ParticleTemplate.Explosion.SpeedVariation, ParticleTemplate.Explosion.SpeedVariation);
            Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            
            var particle = particleSystem.GetParticle();
            particle.Position = position + new Vector2(_random.NextFloat(-radius, radius), _random.NextFloat(-radius, radius));
            particle.Velocity = velocity;
            particle.Color = color;
            particle.Size = ParticleTemplate.Explosion.StartSize + _random.NextFloat(-ParticleTemplate.Explosion.SizeVariation, ParticleTemplate.Explosion.SizeVariation);
            particle.Lifetime = ParticleTemplate.Explosion.Lifetime;
            particle.MaxLifetime = ParticleTemplate.Explosion.Lifetime;
            particle.IsActive = true;
            
            particleSystem.AddParticle(particle);
        }
    }
    
    public static void EmitThrust(Vector2 position, float angle, float effectiveness, ParticleSystem particleSystem)
    {
        int count = (int)(5 * effectiveness);
        for (int i = 0; i < count; i++)
        {
            float spread = _random.NextFloat(-0.3f, 0.3f);
            float speed = ParticleTemplate.Thrust.Speed + _random.NextFloat(-ParticleTemplate.Thrust.SpeedVariation, ParticleTemplate.Thrust.SpeedVariation);
            Vector2 velocity = new Vector2(MathF.Cos(angle + MathF.PI + spread), MathF.Sin(angle + MathF.PI + spread)) * speed;
            
            var particle = particleSystem.GetParticle();
            particle.Position = position;
            particle.Velocity = velocity;
            particle.Color = Color.Lerp(ParticleTemplate.Thrust.StartColor, ParticleTemplate.Thrust.EndColor, _random.NextFloat(0, 1));
            particle.Size = ParticleTemplate.Thrust.StartSize + _random.NextFloat(-ParticleTemplate.Thrust.SizeVariation, ParticleTemplate.Thrust.SizeVariation);
            particle.Lifetime = ParticleTemplate.Thrust.Lifetime;
            particle.MaxLifetime = ParticleTemplate.Thrust.Lifetime;
            particle.IsActive = true;
            
            particleSystem.AddParticle(particle);
        }
    }
    
    public static void EmitShieldHit(Vector2 position, Vector2 direction, ParticleSystem particleSystem)
    {
        for (int i = 0; i < 20; i++)
        {
            float angle = _random.NextFloat(0, (float)(Math.PI * 2));
            float speed = ParticleTemplate.ShieldHit.Speed + _random.NextFloat(-ParticleTemplate.ShieldHit.SpeedVariation, ParticleTemplate.ShieldHit.SpeedVariation);
            Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            
            var particle = particleSystem.GetParticle();
            particle.Position = position;
            particle.Velocity = velocity;
            particle.Color = ParticleTemplate.ShieldHit.StartColor;
            particle.Size = ParticleTemplate.ShieldHit.StartSize + _random.NextFloat(-ParticleTemplate.ShieldHit.SizeVariation, ParticleTemplate.ShieldHit.SizeVariation);
            particle.Lifetime = ParticleTemplate.ShieldHit.Lifetime;
            particle.MaxLifetime = ParticleTemplate.ShieldHit.Lifetime;
            particle.IsActive = true;
            
            particleSystem.AddParticle(particle);
        }
    }
    
    public static void EmitHyperspace(Vector2 from, Vector2 to, ParticleSystem particleSystem)
    {
        // Emit particles along the path
        Vector2 direction = Vector2.Normalize(to - from);
        float distance = Vector2.Distance(from, to);
        int count = (int)(distance / 10);
        
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;
            Vector2 pos = Vector2.Lerp(from, to, t);
            
            float angle = _random.NextFloat(0, (float)(Math.PI * 2));
            float speed = ParticleTemplate.Hyperspace.Speed + _random.NextFloat(-ParticleTemplate.Hyperspace.SpeedVariation, ParticleTemplate.Hyperspace.SpeedVariation);
            Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            
            var particle = particleSystem.GetParticle();
            particle.Position = pos;
            particle.Velocity = velocity;
            particle.Color = ParticleTemplate.Hyperspace.StartColor;
            particle.Size = ParticleTemplate.Hyperspace.StartSize + _random.NextFloat(-ParticleTemplate.Hyperspace.SizeVariation, ParticleTemplate.Hyperspace.SizeVariation);
            particle.Lifetime = ParticleTemplate.Hyperspace.Lifetime;
            particle.MaxLifetime = ParticleTemplate.Hyperspace.Lifetime;
            particle.IsActive = true;
            
            particleSystem.AddParticle(particle);
        }
    }
    
    public static void EmitPowerUpCollect(Vector2 position, ParticleSystem particleSystem)
    {
        for (int i = 0; i < 15; i++)
        {
            float angle = _random.NextFloat(0, (float)(Math.PI * 2));
            float speed = ParticleTemplate.PowerUpCollect.Speed + _random.NextFloat(-ParticleTemplate.PowerUpCollect.SpeedVariation, ParticleTemplate.PowerUpCollect.SpeedVariation);
            Vector2 velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            
            var particle = particleSystem.GetParticle();
            particle.Position = position;
            particle.Velocity = velocity;
            particle.Color = ParticleTemplate.PowerUpCollect.StartColor;
            particle.Size = ParticleTemplate.PowerUpCollect.StartSize + _random.NextFloat(-ParticleTemplate.PowerUpCollect.SizeVariation, ParticleTemplate.PowerUpCollect.SizeVariation);
            particle.Lifetime = ParticleTemplate.PowerUpCollect.Lifetime;
            particle.MaxLifetime = ParticleTemplate.PowerUpCollect.Lifetime;
            particle.IsActive = true;
            
            particleSystem.AddParticle(particle);
        }
    }
}

