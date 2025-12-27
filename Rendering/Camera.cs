using System;
using Microsoft.Xna.Framework;
using static AsteroidsClone.Core.Extensions;

namespace AsteroidsClone.Rendering;

public class Camera
{
    public Vector2 Position { get; set; }
    public float Zoom { get; set; } = 1.0f;
    public float Rotation { get; set; }
    
    private float _shakeTimer;
    private float _shakeIntensity;
    private Vector2 _shakeOffset;
    private Random _random = new Random();
    
    public Matrix Transform
    {
        get
        {
            return Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(Zoom) *
                   Matrix.CreateTranslation(_shakeOffset.X, _shakeOffset.Y, 0);
        }
    }
    
    public void Update(float deltaTime)
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= deltaTime;
            _shakeOffset = new Vector2(
                _random.NextFloat(-_shakeIntensity, _shakeIntensity),
                _random.NextFloat(-_shakeIntensity, _shakeIntensity)
            );
            _shakeIntensity *= 0.9f; // Decay
            
            if (_shakeTimer <= 0)
            {
                _shakeOffset = Vector2.Zero;
            }
        }
    }
    
    public void Shake(float duration, float intensity)
    {
        _shakeTimer = duration;
        _shakeIntensity = intensity;
    }
    
    public void ZoomTo(float targetZoom, float speed)
    {
        Zoom = MathHelper.Lerp(Zoom, targetZoom, speed);
    }
}

