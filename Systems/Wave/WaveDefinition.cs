using System.Collections.Generic;
using AsteroidsClone.Systems.Spawning;

namespace AsteroidsClone.Systems.Wave;

public class WaveDefinition
{
    public int WaveNumber { get; set; }
    public int AsteroidCount { get; set; }
    public List<UFOType> UFOs { get; set; } = new List<UFOType>();
    public WaveModifier Modifier { get; set; }
    public bool HasBoss { get; set; }
}

public class WaveModifier
{
    public float AsteroidSpeedMultiplier { get; set; } = 1.0f;
    public float UFOFrequency { get; set; } = 1.0f;
    public float PowerUpRate { get; set; } = 1.0f;
    public string Name { get; set; } = "";
}


