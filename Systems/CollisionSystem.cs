using System.Linq;
using AsteroidsClone.Audio;
using AsteroidsClone.Core;
using AsteroidsClone.Entities;
using AsteroidsClone.Rendering;
using AsteroidsClone.Rendering.Particles;
using Microsoft.Xna.Framework;

namespace AsteroidsClone.Systems;

public class CollisionSystem
{
    private Camera _camera;

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    private void TriggerShake(float duration, float intensity)
    {
        if (GameSettings.ScreenShakeEnabled && _camera != null)
        {
            _camera.Shake(duration, intensity);
        }
    }

    public void Update()
    {
        // Player bullets vs Asteroids
        foreach (var bullet in GameState.PlayerBullets.Where(b => b.IsActive).ToList())
        {
            foreach (var asteroid in GameState.Asteroids.Where(a => a.IsActive).ToList())
            {
                if (bullet.CollidesWith(asteroid))
                {
                    HandleBulletAsteroidCollision(bullet, asteroid);
                }
            }
        }
        
        // Player bullets vs UFOs
        foreach (var bullet in GameState.PlayerBullets.Where(b => b.IsActive).ToList())
        {
            foreach (var ufo in GameState.UFOs.Where(u => u.IsActive).ToList())
            {
                if (bullet.CollidesWith(ufo))
                {
                    HandleBulletUFOCollision(bullet, ufo);
                }
            }
        }
        
        // Player vs Asteroids
        if (GameState.Player != null && GameState.Player.IsActive && !GameState.Player.IsInvulnerable)
        {
            foreach (var asteroid in GameState.Asteroids.Where(a => a.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(asteroid))
                {
                    HandlePlayerAsteroidCollision(asteroid);
                }
            }
        }
        
        // Player vs UFOs
        if (GameState.Player != null && GameState.Player.IsActive && !GameState.Player.IsInvulnerable)
        {
            foreach (var ufo in GameState.UFOs.Where(u => u.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(ufo))
                {
                    HandlePlayerUFOCollision(ufo);
                }
            }
        }
        
        // Player vs Enemy Bullets
        if (GameState.Player != null && GameState.Player.IsActive && !GameState.Player.IsInvulnerable)
        {
            foreach (var bullet in GameState.EnemyBullets.Where(b => b.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(bullet))
                {
                    HandlePlayerBulletCollision(bullet);
                }
            }
        }
        
        // Player vs Power-ups
        if (GameState.Player != null && GameState.Player.IsActive)
        {
            foreach (var powerUp in GameState.PowerUps.Where(p => p.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(powerUp))
                {
                    powerUp.Apply(GameState.Player);
                    powerUp.IsActive = false;
                    ParticleSystem.Instance?.CreatePowerUpCollectEffect(powerUp.Position);
                }
            }
        }
        
        // Player vs Mines
        if (GameState.Player != null && GameState.Player.IsActive && !GameState.Player.IsInvulnerable)
        {
            foreach (var mine in GameState.Mines.Where(m => m.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(mine))
                {
                    HandlePlayerMineCollision(mine);
                }
            }
        }
        
        // Player vs Drones
        if (GameState.Player != null && GameState.Player.IsActive && !GameState.Player.IsInvulnerable)
        {
            foreach (var drone in GameState.Drones.Where(d => d.IsActive).ToList())
            {
                if (GameState.Player.CollidesWith(drone))
                {
                    HandlePlayerDroneCollision(drone);
                }
            }
        }
    }
    
    private void HandleBulletAsteroidCollision(Bullet bullet, Asteroid asteroid)
    {
        if (!bullet.IsPiercing) bullet.IsActive = false;

        GameState.AddScore(asteroid.PointValue);

        var children = asteroid.Split();
        foreach (var child in children)
        {
            GameState.AddEntity(child);
        }

        asteroid.IsActive = false;
        Systems.Spawning.PowerUpSpawner.OnAsteroidDestroyed(asteroid.Position, asteroid.Size);

        // Explosion effect
        ParticleSystem.Instance?.CreateExplosion(asteroid.Position, asteroid.Radius, Color.White);

        // Screen shake based on asteroid size
        float shakeDuration = asteroid.Size switch
        {
            AsteroidSize.Large => 0.12f,
            AsteroidSize.Medium => 0.08f,
            _ => 0.05f
        };
        float shakeIntensity = asteroid.Size switch
        {
            AsteroidSize.Large => 4f,
            AsteroidSize.Medium => 2f,
            _ => 1f
        };
        TriggerShake(shakeDuration, shakeIntensity);
    }
    
    private void HandleBulletUFOCollision(Bullet bullet, Entities.UFO.UFOBase ufo)
    {
        if (!bullet.IsPiercing) bullet.IsActive = false;

        if (ufo.TakeDamage())
        {
            GameState.AddScore(ufo.PointValue);
            ParticleSystem.Instance?.CreateExplosion(ufo.Position, ufo.Radius, Color.Green);
            ufo.IsActive = false;
            TriggerShake(0.15f, 5f);
        }
    }
    
    private void HandlePlayerAsteroidCollision(Asteroid asteroid)
    {
        Vector2 deathPos = GameState.Player.Position;
        float deathRot = GameState.Player.Rotation;

        if (GameState.Player.TakeDamage())
        {
            ParticleSystem.Instance?.CreateShipDeathEffect(deathPos, deathRot);
            AudioManager.Play(Audio.SoundEffect.Explosion);
            GameState.Player.IsActive = false;
            GameState.Lives--;
            TriggerShake(0.4f, 15f);
        }
        else
        {
            // Shield absorbed the hit
            TriggerShake(0.15f, 6f);
        }
    }
    
    private void HandlePlayerUFOCollision(Entities.UFO.UFOBase ufo)
    {
        Vector2 deathPos = GameState.Player.Position;
        float deathRot = GameState.Player.Rotation;

        if (GameState.Player.TakeDamage())
        {
            ParticleSystem.Instance?.CreateShipDeathEffect(deathPos, deathRot);
            AudioManager.Play(Audio.SoundEffect.Explosion);
            GameState.Player.IsActive = false;
            GameState.Lives--;
            TriggerShake(0.4f, 15f);
        }
        else
        {
            TriggerShake(0.15f, 6f);
        }
    }

    private void HandlePlayerBulletCollision(Bullet bullet)
    {
        bullet.IsActive = false;
        Vector2 deathPos = GameState.Player.Position;
        float deathRot = GameState.Player.Rotation;

        if (GameState.Player.TakeDamage())
        {
            ParticleSystem.Instance?.CreateShipDeathEffect(deathPos, deathRot);
            AudioManager.Play(Audio.SoundEffect.Explosion);
            GameState.Player.IsActive = false;
            GameState.Lives--;
            TriggerShake(0.4f, 15f);
        }
        else
        {
            TriggerShake(0.15f, 6f);
        }
    }

    private void HandlePlayerMineCollision(Mine mine)
    {
        mine.Explode();
        Vector2 deathPos = GameState.Player.Position;
        float deathRot = GameState.Player.Rotation;

        if (GameState.Player.TakeDamage())
        {
            ParticleSystem.Instance?.CreateShipDeathEffect(deathPos, deathRot);
            AudioManager.Play(Audio.SoundEffect.Explosion);
            GameState.Player.IsActive = false;
            GameState.Lives--;
            TriggerShake(0.4f, 15f);
        }
        else
        {
            TriggerShake(0.15f, 6f);
        }
    }

    private void HandlePlayerDroneCollision(Drone drone)
    {
        drone.IsActive = false;
        Vector2 deathPos = GameState.Player.Position;
        float deathRot = GameState.Player.Rotation;

        if (GameState.Player.TakeDamage())
        {
            ParticleSystem.Instance?.CreateShipDeathEffect(deathPos, deathRot);
            AudioManager.Play(Audio.SoundEffect.Explosion);
            GameState.Player.IsActive = false;
            GameState.Lives--;
            TriggerShake(0.4f, 15f);
        }
        else
        {
            TriggerShake(0.15f, 6f);
        }
    }
}

