using Microsoft.Xna.Framework;

namespace AsteroidsClone.Rendering.Particles;

public struct Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public float Size;
    public float Lifetime;
    public float MaxLifetime;
    public bool IsActive;
    
    public float Alpha => Lifetime / MaxLifetime;
}


