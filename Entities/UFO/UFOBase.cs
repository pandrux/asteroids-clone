using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;
using static AsteroidsClone.Core.Extensions;

namespace AsteroidsClone.Entities.UFO;

public abstract class UFOBase : GameObject
{
    public int PointValue { get; protected set; }
    public int Health { get; protected set; }
    public int MaxHealth { get; protected set; }
    public float FireCooldown { get; protected set; }
    protected float _fireTimer;
    protected static Random _random = new Random();
    
    protected UFOBase()
    {
        Radius = 25f;
        _fireTimer = 0;
    }
    
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        _fireTimer = Math.Max(0, _fireTimer - deltaTime);
        UpdateAI(deltaTime);
    }
    
    protected abstract void UpdateAI(float deltaTime);
    
    protected void FireAtPlayer()
    {
        if (GameState.Player == null || !GameState.Player.IsActive) return;
        if (GameState.Player.IsInvulnerable) return; // Don't shoot at invulnerable player

        Vector2 direction = Vector2.Normalize(GameState.Player.Position - Position);
        var bullet = new Entities.Bullet(Position, direction, false);
        GameState.AddEntity(bullet);
    }
    
    public virtual bool TakeDamage()
    {
        Health--;
        return Health <= 0;
    }
    
    public override void Draw(SpriteBatch spriteBatch)
    {
        // Will be implemented with vector rendering
    }
}
