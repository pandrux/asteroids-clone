using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Entities;

namespace AsteroidsClone.Entities.UFO;

public class CarrierUFO : UFOBase
{
    private float _droneSpawnCooldown;
    private const float DroneSpawnInterval = 5.0f;
    private int _dronesSpawned;
    private const int MaxDrones = 3;
    
    public CarrierUFO()
    {
        PointValue = 500;
        Health = 4;
        MaxHealth = 4;
        Radius = 35f;
        FireCooldown = 2.0f;
        _droneSpawnCooldown = DroneSpawnInterval;
        _dronesSpawned = 0;
        
        // Moves slowly
        Vector2 direction = new Vector2(_random.NextFloat(-1f, 1f), _random.NextFloat(-1f, 1f));
        direction.Normalize();
        Velocity = direction * 35f;
    }
    
    protected override void UpdateAI(float deltaTime)
    {
        _droneSpawnCooldown -= deltaTime;
        
        // Spawn drones
        if (_droneSpawnCooldown <= 0 && _dronesSpawned < MaxDrones)
        {
            SpawnDrone();
            _droneSpawnCooldown = DroneSpawnInterval;
            _dronesSpawned++;
        }
        
        // Fire at player
        if (_fireTimer <= 0)
        {
            FireAtPlayer();
            _fireTimer = FireCooldown + _random.NextFloat(-0.5f, 0.5f);
        }
    }
    
    private void SpawnDrone()
    {
        Vector2 spawnOffset = new Vector2(
            _random.NextFloat(-50f, 50f),
            _random.NextFloat(-50f, 50f)
        );
        Vector2 dronePosition = Position + spawnOffset;
        Vector2 droneVelocity = new Vector2(
            _random.NextFloat(-100f, 100f),
            _random.NextFloat(-100f, 100f)
        );
        
        var drone = new Drone(dronePosition, droneVelocity);
        GameState.AddEntity(drone);
    }
}
