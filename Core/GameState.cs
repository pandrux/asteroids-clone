using System.Collections.Generic;
using System.Linq;
using AsteroidsClone.Entities;
using AsteroidsClone.Entities.UFO;

namespace AsteroidsClone.Core;

public static class GameState
{
    // Entity collections
    public static Ship Player;
    public static List<Asteroid> Asteroids = new();
    public static List<Bullet> PlayerBullets = new();
    public static List<Bullet> EnemyBullets = new();
    public static List<UFOBase> UFOs = new();
    public static List<PowerUp> PowerUps = new();
    public static List<Mine> Mines = new();
    public static List<Drone> Drones = new();
    
    // Game info
    public static int Score { get; private set; }
    public static int Lives { get; set; }
    public static int CurrentWave { get; set; }
    public static float ScoreMultiplier { get; set; } = 1.0f;
    public static bool HasActiveBoss => UFOs.Any(u => u is BossUFO && u.IsActive);
    private static int _extraLivesAwarded = 0;
    
    // Screen dimensions
    public static int ScreenWidth { get; set; } = 1280;
    public static int ScreenHeight { get; set; } = 720;
    
    public static void AddEntity(GameObject entity)
    {
        switch (entity)
        {
            case Asteroid a: Asteroids.Add(a); break;
            case Bullet b when b.IsPlayerBullet: PlayerBullets.Add(b); break;
            case Bullet b: EnemyBullets.Add(b); break;
            case UFOBase u: UFOs.Add(u); break;
            case PowerUp p: PowerUps.Add(p); break;
            case Mine m: Mines.Add(m); break;
            case Drone d: Drones.Add(d); break;
        }
    }
    
    public static void RemoveEntity(GameObject entity)
    {
        entity.IsActive = false;
    }
    
    public static void AddScore(int points)
    {
        Score += (int)(points * ScoreMultiplier);
        CheckExtraLife();
    }
    
    private static void CheckExtraLife()
    {
        // Extra life every 10,000 points
        int extraLifeThreshold = 10000;
        int livesEarned = Score / extraLifeThreshold;
        if (livesEarned > _extraLivesAwarded)
        {
            Lives++;
            _extraLivesAwarded = livesEarned;
        }
    }
    
    public static void Reset()
    {
        Score = 0;
        Lives = 3;
        CurrentWave = 0;
        ScoreMultiplier = 1.0f;
        _extraLivesAwarded = 0;
        ClearAllEntities();
    }
    
    public static void DestroyAllAsteroids()
    {
        foreach (var asteroid in Asteroids)
        {
            asteroid.IsActive = false;
            // ParticleSystem.CreateExplosion(asteroid.Position, 30f, Color.White);
        }
    }
    
    private static void ClearAllEntities()
    {
        Asteroids.Clear();
        PlayerBullets.Clear();
        EnemyBullets.Clear();
        UFOs.Clear();
        PowerUps.Clear();
        Mines.Clear();
        Drones.Clear();
    }
    
    public static void CleanupInactiveEntities()
    {
        Asteroids.RemoveAll(a => !a.IsActive);
        PlayerBullets.RemoveAll(b => !b.IsActive);
        EnemyBullets.RemoveAll(b => !b.IsActive);
        UFOs.RemoveAll(u => !u.IsActive);
        PowerUps.RemoveAll(p => !p.IsActive);
        Mines.RemoveAll(m => !m.IsActive);
        Drones.RemoveAll(d => !d.IsActive);
    }
}

