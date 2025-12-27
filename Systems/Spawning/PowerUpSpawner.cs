using System;
using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Entities;

namespace AsteroidsClone.Systems.Spawning;

public static class PowerUpSpawner
{
    private static Random _random = new Random();
    private const float DropChance = 0.15f; // 15% chance to drop from destroyed asteroid
    
    public static void OnAsteroidDestroyed(Vector2 position, AsteroidSize size)
    {
        // Higher chance for larger asteroids
        float chance = size switch
        {
            AsteroidSize.Large => DropChance * 1.5f,
            AsteroidSize.Medium => DropChance,
            AsteroidSize.Small => DropChance * 0.5f,
            _ => DropChance
        };
        
        if (_random.NextDouble() < chance)
        {
            SpawnRandomPowerUp(position);
        }
    }
    
    public static void SpawnRandomPowerUp(Vector2 position)
    {
        // Random power-up type
        var types = Enum.GetValues<PowerUpType>();
        PowerUpType type = types[_random.Next(types.Length)];
        
        var powerUp = new PowerUp(type, position);
        GameState.AddEntity(powerUp);
    }
    
    public static void SpawnPowerUp(PowerUpType type, Vector2 position)
    {
        var powerUp = new PowerUp(type, position);
        GameState.AddEntity(powerUp);
    }
}


