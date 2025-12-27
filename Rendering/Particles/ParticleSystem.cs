using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.Rendering.Particles;

// Debris for ship death effect - line segments that spin and drift
public struct Debris
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Vector2 Start;  // Line segment start (relative to position)
    public Vector2 End;    // Line segment end (relative to position)
    public float Rotation;
    public float RotationSpeed;
    public float Lifetime;
    public float MaxLifetime;
    public bool IsActive;

    public float Alpha => Lifetime / MaxLifetime;
}

public class ParticleSystem
{
    private List<Particle> _particles = new List<Particle>();
    private List<Debris> _debris = new List<Debris>();
    private ParticlePool _pool;
    private const int MaxParticles = 10000;
    private SpriteBatch _spriteBatch;
    private Texture2D _particleTexture;
    private Random _random = new Random();

    // Static instance for easy access from game systems
    public static ParticleSystem Instance { get; private set; }

    public ParticleSystem(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        _spriteBatch = spriteBatch;
        _pool = new ParticlePool(MaxParticles);

        // Create a simple 1x1 white texture for particles
        _particleTexture = new Texture2D(graphicsDevice, 1, 1);
        _particleTexture.SetData(new[] { Color.White });

        Instance = this;
    }
    
    public void Update(float deltaTime)
    {
        // Update particles
        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            var particle = _particles[i];

            if (!particle.IsActive)
            {
                _pool.Return(particle);
                _particles.RemoveAt(i);
                continue;
            }

            particle.Position += particle.Velocity * deltaTime;
            particle.Lifetime -= deltaTime;

            if (particle.Lifetime <= 0)
            {
                particle.IsActive = false;
                _pool.Return(particle);
                _particles.RemoveAt(i);
            }
            else
            {
                _particles[i] = particle;
            }
        }

        // Update debris (ship death line segments)
        for (int i = _debris.Count - 1; i >= 0; i--)
        {
            var debris = _debris[i];

            debris.Position += debris.Velocity * deltaTime;
            debris.Rotation += debris.RotationSpeed * deltaTime;
            debris.Lifetime -= deltaTime;

            if (debris.Lifetime <= 0)
            {
                _debris.RemoveAt(i);
            }
            else
            {
                _debris[i] = debris;
            }
        }
    }
    
    public void Draw()
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        
        foreach (var particle in _particles.Where(p => p.IsActive))
        {
            Color color = particle.Color * particle.Alpha;
            float size = particle.Size * (1f - particle.Alpha);
            
            _spriteBatch.Draw(
                _particleTexture,
                particle.Position,
                null,
                color,
                0f,
                Vector2.Zero,
                size,
                SpriteEffects.None,
                0f
            );
        }
        
        _spriteBatch.End();
    }

    // Draw debris using VectorRenderer (call this within VectorRenderer.Begin/End)
    public void DrawDebris()
    {
        foreach (var debris in _debris)
        {
            // Rotate the line segment endpoints
            float cos = MathF.Cos(debris.Rotation);
            float sin = MathF.Sin(debris.Rotation);

            Vector2 rotatedStart = new Vector2(
                debris.Start.X * cos - debris.Start.Y * sin,
                debris.Start.X * sin + debris.Start.Y * cos
            );
            Vector2 rotatedEnd = new Vector2(
                debris.End.X * cos - debris.End.Y * sin,
                debris.End.X * sin + debris.End.Y * cos
            );

            Vector2 worldStart = debris.Position + rotatedStart;
            Vector2 worldEnd = debris.Position + rotatedEnd;

            Color color = Color.White * debris.Alpha;
            VectorRenderer.DrawLine(worldStart, worldEnd, color);
        }
    }

    public Particle GetParticle()
    {
        return _pool.Get();
    }
    
    public void AddParticle(Particle particle)
    {
        if (_particles.Count < MaxParticles)
        {
            _particles.Add(particle);
        }
        else
        {
            _pool.Return(particle);
        }
    }
    
    public void CreateExplosion(Vector2 position, float radius, Color color)
    {
        int particleCount = (int)(radius * 2);
        for (int i = 0; i < particleCount; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float speed = (float)(_random.NextDouble() * 200 + 50);
            float lifetime = (float)(_random.NextDouble() * 0.5 + 0.3);

            var particle = _pool.Get();
            particle.Position = position;
            particle.Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            particle.Color = color;
            particle.Size = (float)(_random.NextDouble() * 4 + 2);
            particle.Lifetime = lifetime;
            particle.MaxLifetime = lifetime;
            particle.IsActive = true;
            AddParticle(particle);
        }
    }

    public void CreateThrustEffect(Vector2 position, float angle, float effectiveness)
    {
        if (effectiveness <= 0) return;

        int particleCount = (int)(8 * effectiveness);
        for (int i = 0; i < particleCount; i++)
        {
            // Particles shoot out opposite to thrust direction with some spread
            float spread = (float)(_random.NextDouble() - 0.5) * 0.8f;
            float particleAngle = angle + MathF.PI + spread;
            float speed = (float)(_random.NextDouble() * 150 + 80) * effectiveness;

            var particle = _pool.Get();
            particle.Position = position;
            particle.Velocity = new Vector2(MathF.Cos(particleAngle), MathF.Sin(particleAngle)) * speed;
            particle.Color = Color.Lerp(Color.Orange, Color.Yellow, (float)_random.NextDouble());
            particle.Size = (float)(_random.NextDouble() * 5 + 3);
            particle.Lifetime = 0.3f;
            particle.MaxLifetime = 0.3f;
            particle.IsActive = true;
            AddParticle(particle);
        }
    }

    public void CreateShieldHitEffect(Vector2 position, Vector2 direction)
    {
        int particleCount = 30;
        for (int i = 0; i < particleCount; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float speed = (float)(_random.NextDouble() * 150 + 50);

            var particle = _pool.Get();
            particle.Position = position;
            particle.Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            particle.Color = Color.Lerp(Color.Cyan, Color.White, (float)_random.NextDouble());
            particle.Size = (float)(_random.NextDouble() * 6 + 4);
            particle.Lifetime = 0.5f;
            particle.MaxLifetime = 0.5f;
            particle.IsActive = true;
            AddParticle(particle);
        }
    }

    public void CreateHyperspaceEffect(Vector2 from, Vector2 to)
    {
        // Disappear effect at origin
        for (int i = 0; i < 30; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float speed = (float)(_random.NextDouble() * 150 + 50);

            var particle = _pool.Get();
            particle.Position = from;
            particle.Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            particle.Color = Color.Purple;
            particle.Size = (float)(_random.NextDouble() * 4 + 2);
            particle.Lifetime = 0.4f;
            particle.MaxLifetime = 0.4f;
            particle.IsActive = true;
            AddParticle(particle);
        }

        // Appear effect at destination
        for (int i = 0; i < 30; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float speed = (float)(_random.NextDouble() * 150 + 50);

            var particle = _pool.Get();
            particle.Position = to;
            particle.Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            particle.Color = Color.Magenta;
            particle.Size = (float)(_random.NextDouble() * 4 + 2);
            particle.Lifetime = 0.4f;
            particle.MaxLifetime = 0.4f;
            particle.IsActive = true;
            AddParticle(particle);
        }
    }

    public void CreatePowerUpCollectEffect(Vector2 position)
    {
        int particleCount = 20;
        for (int i = 0; i < particleCount; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float speed = (float)(_random.NextDouble() * 80 + 40);

            var particle = _pool.Get();
            particle.Position = position;
            particle.Velocity = new Vector2(MathF.Cos(angle), MathF.Sin(angle)) * speed;
            particle.Color = Color.Lerp(Color.Yellow, Color.Green, (float)_random.NextDouble());
            particle.Size = (float)(_random.NextDouble() * 3 + 2);
            particle.Lifetime = 0.5f;
            particle.MaxLifetime = 0.5f;
            particle.IsActive = true;
            AddParticle(particle);
        }
    }

    public void CreateShipDeathEffect(Vector2 position, float rotation)
    {
        // Ship shape vertices (same as VectorShapes.ShipShape)
        Vector2 nose = new Vector2(15, 0);
        Vector2 backTop = new Vector2(-10, -10);
        Vector2 backBottom = new Vector2(-10, 10);

        // Rotate vertices to match ship's orientation
        float cos = MathF.Cos(rotation);
        float sin = MathF.Sin(rotation);

        Vector2 RotatePoint(Vector2 p) => new Vector2(
            p.X * cos - p.Y * sin,
            p.X * sin + p.Y * cos
        );

        Vector2 rotNose = RotatePoint(nose);
        Vector2 rotBackTop = RotatePoint(backTop);
        Vector2 rotBackBottom = RotatePoint(backBottom);

        // Create 3 debris pieces - one for each line segment
        // Each drifts in a different direction and spins

        // Segment 1: Nose to Back Top
        Vector2 seg1Center = (rotNose + rotBackTop) / 2;
        _debris.Add(new Debris
        {
            Position = position + seg1Center,
            Velocity = new Vector2((float)(_random.NextDouble() - 0.5) * 100, (float)(_random.NextDouble() - 0.5) * 100),
            Start = (rotNose - seg1Center) / 2,
            End = (rotBackTop - seg1Center) / 2,
            Rotation = 0,
            RotationSpeed = (float)(_random.NextDouble() - 0.5) * 8,
            Lifetime = 2.5f,
            MaxLifetime = 2.5f,
            IsActive = true
        });

        // Segment 2: Back Top to Back Bottom
        Vector2 seg2Center = (rotBackTop + rotBackBottom) / 2;
        _debris.Add(new Debris
        {
            Position = position + seg2Center,
            Velocity = new Vector2((float)(_random.NextDouble() - 0.5) * 100, (float)(_random.NextDouble() - 0.5) * 100),
            Start = (rotBackTop - seg2Center) / 2,
            End = (rotBackBottom - seg2Center) / 2,
            Rotation = 0,
            RotationSpeed = (float)(_random.NextDouble() - 0.5) * 8,
            Lifetime = 2.5f,
            MaxLifetime = 2.5f,
            IsActive = true
        });

        // Segment 3: Back Bottom to Nose
        Vector2 seg3Center = (rotBackBottom + rotNose) / 2;
        _debris.Add(new Debris
        {
            Position = position + seg3Center,
            Velocity = new Vector2((float)(_random.NextDouble() - 0.5) * 100, (float)(_random.NextDouble() - 0.5) * 100),
            Start = (rotBackBottom - seg3Center) / 2,
            End = (rotNose - seg3Center) / 2,
            Rotation = 0,
            RotationSpeed = (float)(_random.NextDouble() - 0.5) * 8,
            Lifetime = 2.5f,
            MaxLifetime = 2.5f,
            IsActive = true
        });

        // Also add some explosion particles
        CreateExplosion(position, 20, Color.White);
    }
}


