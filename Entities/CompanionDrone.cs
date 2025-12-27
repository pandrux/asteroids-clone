using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;
using AsteroidsClone.Entities.UFO;

namespace AsteroidsClone.Entities;

public class CompanionDrone : GameObject
{
    public DroneStrategy Strategy { get; set; }

    private float _fireTimer;
    private float _orbitAngle;
    private GameObject _currentTarget;
    private const float BulletSpeed = 600f;
    private static Random _random = new Random();

    private float GetMoveSpeed()
    {
        return Strategy.Behavior switch
        {
            BehaviorMode.Aggressive => 500f,  // Fast, chasing
            BehaviorMode.Defensive => 400f,   // Quick to return
            BehaviorMode.Balanced => 350f,
            BehaviorMode.Conserve => 250f,    // Slow, energy saving
            BehaviorMode.Evasive => 450f,     // Quick dodging
            _ => 350f
        };
    }

    public CompanionDrone()
    {
        Strategy = new DroneStrategy();
        Radius = 8f;
        _orbitAngle = 0f;
    }

    public override void Update(float deltaTime)
    {
        if (!IsActive) return;

        UpdatePositioning(deltaTime);
        UpdateTargeting();
        TryFire(deltaTime);
        WrapPosition(GameState.ScreenWidth, GameState.ScreenHeight);
    }

    private void UpdatePositioning(float deltaTime)
    {
        Vector2 desiredPosition = CalculateDesiredPosition(deltaTime);
        Vector2 toDesired = desiredPosition - Position;
        float distance = toDesired.Length();

        if (distance > 5f)
        {
            Vector2 direction = Vector2.Normalize(toDesired);
            float speed = Math.Min(GetMoveSpeed() * deltaTime, distance);
            Position += direction * speed;

            // Rotate to face movement direction
            Rotation = MathF.Atan2(direction.Y, direction.X);
        }

        // If we have a target, rotate to face it
        if (_currentTarget != null && _currentTarget.IsActive)
        {
            Vector2 toTarget = _currentTarget.Position - Position;
            Rotation = MathF.Atan2(toTarget.Y, toTarget.X);
        }
    }

    private Vector2 CalculateDesiredPosition(float deltaTime)
    {
        if (GameState.Player == null || !GameState.Player.IsActive)
            return Position;

        Ship player = GameState.Player;
        float orbitDistance = GetOrbitDistance();

        switch (Strategy.Positioning)
        {
            case PositioningMode.OrbitPlayer:
                // Orbit speed varies by behavior
                float orbitSpeed = Strategy.Behavior == BehaviorMode.Aggressive ? 120f :
                                   Strategy.Behavior == BehaviorMode.Evasive ? 90f : 60f;
                _orbitAngle += MathHelper.ToRadians(orbitSpeed) * deltaTime;
                return player.Position + new Vector2(
                    MathF.Cos(_orbitAngle) * orbitDistance,
                    MathF.Sin(_orbitAngle) * orbitDistance);

            case PositioningMode.LeadAhead:
                if (player.Velocity.LengthSquared() > 0.1f)
                {
                    return player.Position + Vector2.Normalize(player.Velocity) * orbitDistance;
                }
                return player.Position + new Vector2(orbitDistance, 0);

            case PositioningMode.RearGuard:
                Vector2 facing = new Vector2(MathF.Cos(player.Rotation), MathF.Sin(player.Rotation));
                return player.Position - facing * orbitDistance;

            case PositioningMode.FlankLeft:
                Vector2 leftPerp = new Vector2(-MathF.Sin(player.Rotation), MathF.Cos(player.Rotation));
                return player.Position + leftPerp * orbitDistance;

            case PositioningMode.FlankRight:
                Vector2 rightPerp = new Vector2(MathF.Sin(player.Rotation), -MathF.Cos(player.Rotation));
                return player.Position + rightPerp * orbitDistance;

            case PositioningMode.Intercept:
                if (_currentTarget != null && _currentTarget.IsActive)
                {
                    return _currentTarget.Position;
                }
                goto case PositioningMode.OrbitPlayer;

            default:
                return player.Position;
        }
    }

    private float GetOrbitDistance()
    {
        return Strategy.Behavior switch
        {
            BehaviorMode.Aggressive => 150f,   // Far out, hunting
            BehaviorMode.Defensive => 40f,     // Very close to player
            BehaviorMode.Balanced => 80f,
            BehaviorMode.Conserve => 100f,
            BehaviorMode.Evasive => 200f,      // Very far, dodging
            _ => 80f
        };
    }

    private void UpdateTargeting()
    {
        _currentTarget = FindBestTarget();
    }

    private GameObject FindBestTarget()
    {
        var asteroids = GameState.Asteroids.Where(a => a.IsActive).Cast<GameObject>();
        var ufos = GameState.UFOs.Where(u => u.IsActive).Cast<GameObject>();
        var allTargets = asteroids.Concat(ufos).ToList();

        if (allTargets.Count == 0) return null;

        switch (Strategy.Targeting)
        {
            case TargetingMode.NearestThreat:
                return allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();

            case TargetingMode.LargestFirst:
                // Prefer large asteroids, then medium, then small, then UFOs
                var largeAsteroids = GameState.Asteroids.Where(a => a.IsActive && a.Size == AsteroidSize.Large);
                if (largeAsteroids.Any())
                    return largeAsteroids.OrderBy(a => Vector2.DistanceSquared(Position, a.Position)).First();
                var mediumAsteroids = GameState.Asteroids.Where(a => a.IsActive && a.Size == AsteroidSize.Medium);
                if (mediumAsteroids.Any())
                    return mediumAsteroids.OrderBy(a => Vector2.DistanceSquared(Position, a.Position)).First();
                return allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();

            case TargetingMode.UFOPriority:
                var activeUFOs = GameState.UFOs.Where(u => u.IsActive);
                if (activeUFOs.Any())
                    return activeUFOs.OrderBy(u => Vector2.DistanceSquared(Position, u.Position)).First();
                return allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();

            case TargetingMode.ProtectPlayer:
                if (GameState.Player == null || !GameState.Player.IsActive)
                    return allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();
                // Find threats heading toward player
                return allTargets
                    .Where(t => IsHeadingToward(t, GameState.Player))
                    .OrderBy(t => Vector2.DistanceSquared(GameState.Player.Position, t.Position))
                    .FirstOrDefault()
                    ?? allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();

            case TargetingMode.ClosingFast:
                return allTargets
                    .OrderByDescending(t => GetClosingSpeed(t))
                    .FirstOrDefault();

            default:
                return allTargets.OrderBy(t => Vector2.DistanceSquared(Position, t.Position)).FirstOrDefault();
        }
    }

    private bool IsHeadingToward(GameObject threat, GameObject target)
    {
        if (threat.Velocity.LengthSquared() < 0.1f) return false;
        Vector2 toTarget = target.Position - threat.Position;
        float dot = Vector2.Dot(Vector2.Normalize(threat.Velocity), Vector2.Normalize(toTarget));
        return dot > 0.5f; // Within ~60 degrees
    }

    private float GetClosingSpeed(GameObject target)
    {
        Vector2 relativePos = target.Position - Position;
        Vector2 relativeVel = target.Velocity - Velocity;
        if (relativePos.LengthSquared() < 0.1f) return 0;
        return Vector2.Dot(relativeVel, Vector2.Normalize(relativePos));
    }

    private void TryFire(float deltaTime)
    {
        _fireTimer = Math.Max(0, _fireTimer - deltaTime);

        if (_fireTimer > 0) return;
        if (_currentTarget == null || !_currentTarget.IsActive) return;

        float distanceToTarget = Vector2.Distance(Position, _currentTarget.Position);
        float maxRange = Strategy.Behavior == BehaviorMode.Aggressive ? 600f : 400f;
        if (distanceToTarget > maxRange) return; // Don't shoot at very distant targets

        // Calculate lead position
        Vector2 leadPosition = CalculateLeadPosition(_currentTarget);
        Vector2 direction = Vector2.Normalize(leadPosition - Position);

        // Create bullet
        var bullet = new Bullet(Position + direction * 15f, direction, true);
        GameState.AddEntity(bullet);

        _fireTimer = GetFireCooldown();
    }

    private Vector2 CalculateLeadPosition(GameObject target)
    {
        float distance = Vector2.Distance(Position, target.Position);
        float bulletTravelTime = distance / BulletSpeed;
        return target.Position + target.Velocity * bulletTravelTime;
    }

    private float GetFireCooldown()
    {
        return Strategy.Behavior switch
        {
            BehaviorMode.Aggressive => 0.15f,  // Rapid fire!
            BehaviorMode.Defensive => 0.4f,
            BehaviorMode.Balanced => 0.35f,
            BehaviorMode.Conserve => 1.5f,     // Very slow
            BehaviorMode.Evasive => 0.5f,
            _ => 0.35f
        };
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        // Rendering handled by VectorRenderer
    }
}
