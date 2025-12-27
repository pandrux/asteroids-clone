using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Entities;
using static AsteroidsClone.Core.Extensions;

namespace AsteroidsClone.Systems.Spawning;

public static class AsteroidSpawner
{
    private static Random _random = new Random();
    
    public static void SpawnWave(int waveNumber, int asteroidCount)
    {
        // Spawn asteroids around the edges of the screen
        for (int i = 0; i < asteroidCount; i++)
        {
            var asteroid = SpawnAsteroid(AsteroidSize.Large);
            GameState.AddEntity(asteroid);
        }
    }
    
    public static Asteroid SpawnAsteroid(AsteroidSize size)
    {
        // Spawn at random edge position
        Vector2 position;
        Vector2 velocity;
        
        int side = _random.Next(4); // 0=top, 1=right, 2=bottom, 3=left
        
        switch (side)
        {
            case 0: // Top
                position = new Vector2(
                    _random.Next(0, GameState.ScreenWidth),
                    -20
                );
                velocity = new Vector2(
                    _random.Next(-100, 100),
                    _random.Next(50, 150)
                );
                break;
            case 1: // Right
                position = new Vector2(
                    GameState.ScreenWidth + 20,
                    _random.Next(0, GameState.ScreenHeight)
                );
                velocity = new Vector2(
                    _random.Next(-150, -50),
                    _random.Next(-100, 100)
                );
                break;
            case 2: // Bottom
                position = new Vector2(
                    _random.Next(0, GameState.ScreenWidth),
                    GameState.ScreenHeight + 20
                );
                velocity = new Vector2(
                    _random.Next(-100, 100),
                    _random.Next(-150, -50)
                );
                break;
            default: // Left
                position = new Vector2(
                    -20,
                    _random.Next(0, GameState.ScreenHeight)
                );
                velocity = new Vector2(
                    _random.Next(50, 150),
                    _random.Next(-100, 100)
                );
                break;
        }
        
        // Add some rotation
        var asteroid = new Asteroid(size, position, velocity);
        asteroid.Rotation = _random.NextFloat(0, (float)(Math.PI * 2));
        
        return asteroid;
    }
    
    public static void SpawnInitialWave()
    {
        // Start with 4 large asteroids
        SpawnWave(1, 4);
    }
}

