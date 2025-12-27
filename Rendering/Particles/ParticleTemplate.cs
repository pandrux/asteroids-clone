using Microsoft.Xna.Framework;

namespace AsteroidsClone.Rendering.Particles;

public class ParticleTemplate
{
    public Color StartColor { get; set; }
    public Color EndColor { get; set; }
    public float StartSize { get; set; }
    public float EndSize { get; set; }
    public float Lifetime { get; set; }
    public float Speed { get; set; }
    public float SpeedVariation { get; set; }
    public float SizeVariation { get; set; }
    
    public static ParticleTemplate Explosion => new ParticleTemplate
    {
        StartColor = Color.White,
        EndColor = Color.Orange,
        StartSize = 5f,
        EndSize = 0f,
        Lifetime = 0.5f,
        Speed = 200f,
        SpeedVariation = 100f,
        SizeVariation = 2f
    };
    
    public static ParticleTemplate Thrust => new ParticleTemplate
    {
        StartColor = Color.Cyan,
        EndColor = Color.Blue,
        StartSize = 3f,
        EndSize = 0f,
        Lifetime = 0.3f,
        Speed = 150f,
        SpeedVariation = 50f,
        SizeVariation = 1f
    };
    
    public static ParticleTemplate ShieldHit => new ParticleTemplate
    {
        StartColor = Color.LightBlue,
        EndColor = Color.White,
        StartSize = 8f,
        EndSize = 0f,
        Lifetime = 0.4f,
        Speed = 100f,
        SpeedVariation = 50f,
        SizeVariation = 3f
    };
    
    public static ParticleTemplate Hyperspace => new ParticleTemplate
    {
        StartColor = Color.Purple,
        EndColor = Color.Transparent,
        StartSize = 10f,
        EndSize = 0f,
        Lifetime = 1.0f,
        Speed = 300f,
        SpeedVariation = 150f,
        SizeVariation = 5f
    };
    
    public static ParticleTemplate PowerUpCollect => new ParticleTemplate
    {
        StartColor = Color.Gold,
        EndColor = Color.Yellow,
        StartSize = 6f,
        EndSize = 0f,
        Lifetime = 0.6f,
        Speed = 80f,
        SpeedVariation = 40f,
        SizeVariation = 2f
    };
}


