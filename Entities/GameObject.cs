using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Entities;

public abstract class GameObject
{
    private static int _nextId = 0;
    
    // Identity
    public int Id { get; } = _nextId++;
    
    // Transform
    public Vector2 Position { get; set; }
    public Vector2 Velocity { get; set; }
    public float Rotation { get; set; }
    
    // Collision
    public float Radius { get; protected set; }
    public bool IsActive { get; set; } = true;
    
    public virtual void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        WrapPosition(Core.GameState.ScreenWidth, Core.GameState.ScreenHeight);
    }
    
    public abstract void Draw(SpriteBatch spriteBatch);
    
    protected void WrapPosition(int screenWidth, int screenHeight)
    {
        if (Position.X < 0) Position = new Vector2(Position.X + screenWidth, Position.Y);
        if (Position.X > screenWidth) Position = new Vector2(Position.X - screenWidth, Position.Y);
        if (Position.Y < 0) Position = new Vector2(Position.X, Position.Y + screenHeight);
        if (Position.Y > screenHeight) Position = new Vector2(Position.X, Position.Y - screenHeight);
    }
    
    public bool CollidesWith(GameObject other)
    {
        if (!IsActive || !other.IsActive) return false;
        float distance = Vector2.Distance(Position, other.Position);
        return distance < (Radius + other.Radius);
    }
}


