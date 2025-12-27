using System;
using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Entities.UFO;

namespace AsteroidsClone.Systems.Spawning;

public static class UFOSpawner
{
    private static Random _random = new Random();
    
    public static UFOBase SpawnUFO(UFOType type)
    {
        Vector2 position = GetRandomEdgePosition();
        
        return type switch
        {
            UFOType.Large => new LargeUFO { Position = position },
            UFOType.Small => new SmallUFO { Position = position },
            UFOType.Hunter => new HunterUFO { Position = position },
            UFOType.Bomber => new BomberUFO { Position = position },
            UFOType.Shielded => new ShieldedUFO { Position = position },
            UFOType.Phase => new PhaseUFO { Position = position },
            UFOType.Carrier => new CarrierUFO { Position = position },
            UFOType.Boss => new BossUFO { Position = position },
            _ => new LargeUFO { Position = position }
        };
    }
    
    private static Vector2 GetRandomEdgePosition()
    {
        int side = _random.Next(4);
        return side switch
        {
            0 => new Vector2(_random.Next(0, GameState.ScreenWidth), -30), // Top
            1 => new Vector2(GameState.ScreenWidth + 30, _random.Next(0, GameState.ScreenHeight)), // Right
            2 => new Vector2(_random.Next(0, GameState.ScreenWidth), GameState.ScreenHeight + 30), // Bottom
            _ => new Vector2(-30, _random.Next(0, GameState.ScreenHeight)) // Left
        };
    }
}

public enum UFOType
{
    Large,
    Small,
    Hunter,
    Bomber,
    Shielded,
    Phase,
    Carrier,
    Boss
}


