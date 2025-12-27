using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Rendering;

public static class VectorRenderer
{
    private static GraphicsDevice _graphicsDevice;
    private static BasicEffect _basicEffect;
    private static VertexPositionColor[] _vertices;
    private static int _vertexCount;
    private const int MaxVertices = 10000;
    
    public static void Initialize(GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _basicEffect = new BasicEffect(graphicsDevice)
        {
            VertexColorEnabled = true,
            World = Matrix.Identity,
            View = Matrix.Identity,
            Projection = Matrix.CreateOrthographicOffCenter(
                0, graphicsDevice.Viewport.Width,
                graphicsDevice.Viewport.Height, 0,
                0, 1
            )
        };
        _vertices = new VertexPositionColor[MaxVertices];
        _vertexCount = 0;
    }
    
    public static void Begin(Camera camera = null)
    {
        _vertexCount = 0;
        if (camera != null)
        {
            _basicEffect.View = camera.Transform;
        }
    }
    
    public static void End()
    {
        if (_vertexCount == 0) return;
        
        foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _graphicsDevice.DrawUserPrimitives(
                PrimitiveType.LineList,
                _vertices,
                0,
                _vertexCount / 2
            );
        }
    }
    
    public static void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        if (_vertexCount + 2 > MaxVertices)
        {
            End();
            Begin();
        }
        
        _vertices[_vertexCount++] = new VertexPositionColor(
            new Vector3(start.X, start.Y, 0),
            color
        );
        _vertices[_vertexCount++] = new VertexPositionColor(
            new Vector3(end.X, end.Y, 0),
            color
        );
    }
    
    public static void DrawShape(List<Vector2> points, Vector2 position, float rotation, Color color, bool closed = true)
    {
        if (points.Count < 2) return;
        
        for (int i = 0; i < points.Count; i++)
        {
            Vector2 p1 = RotatePoint(points[i], rotation) + position;
            Vector2 p2;
            
            if (i < points.Count - 1)
            {
                p2 = RotatePoint(points[i + 1], rotation) + position;
            }
            else if (closed)
            {
                p2 = RotatePoint(points[0], rotation) + position;
            }
            else
            {
                break;
            }
            
            DrawLine(p1, p2, color);
        }
    }
    
    public static void DrawShip(Vector2 position, float rotation, bool isThrusting, bool isShielded, bool isInvulnerable = false)
    {
        if (isInvulnerable)
        {
            // Draw glowing aura effect - multiple layers of the ship shape, larger and colored
            DrawShapeScaled(VectorShapes.ShipShape, position, rotation, Color.Cyan * 0.3f, 1.6f);
            DrawShapeScaled(VectorShapes.ShipShape, position, rotation, Color.Cyan * 0.5f, 1.3f);
            DrawShape(VectorShapes.ShipShape, position, rotation, Color.Cyan);
        }
        else
        {
            DrawShape(VectorShapes.ShipShape, position, rotation, Color.White);
        }

        if (isThrusting)
        {
            // Draw thrust effect
            Vector2 thrustPos = position - new Vector2(
                MathF.Cos(rotation),
                MathF.Sin(rotation)
            ) * 15f;
            DrawLine(
                thrustPos,
                thrustPos - new Vector2(MathF.Cos(rotation), MathF.Sin(rotation)) * 10f,
                Color.Cyan
            );
        }

        if (isShielded)
        {
            // Draw shield circle
            DrawCircle(position, 20f, Color.LightBlue, 16);
        }
    }

    public static void DrawShapeScaled(List<Vector2> points, Vector2 position, float rotation, Color color, float scale)
    {
        if (points.Count < 2) return;

        for (int i = 0; i < points.Count; i++)
        {
            Vector2 p1 = RotatePoint(points[i] * scale, rotation) + position;
            Vector2 p2;

            if (i < points.Count - 1)
            {
                p2 = RotatePoint(points[i + 1] * scale, rotation) + position;
            }
            else
            {
                p2 = RotatePoint(points[0] * scale, rotation) + position;
            }

            DrawLine(p1, p2, color);
        }
    }
    
    public static void DrawAsteroid(Vector2 position, float rotation, float radius, Color color)
    {
        List<Vector2> shape = radius switch
        {
            > 30f => VectorShapes.LargeAsteroidShape,
            > 15f => VectorShapes.MediumAsteroidShape,
            _ => VectorShapes.SmallAsteroidShape
        };
        
        DrawShape(shape, position, rotation, color);
    }
    
    public static void DrawUFO(Vector2 position, float radius, Color color)
    {
        List<Vector2> shape = radius > 30f ? VectorShapes.BossUFOShape : VectorShapes.UFOShape;
        DrawShape(shape, position, 0, color);
    }
    
    public static void DrawBullet(Vector2 position, Vector2 direction, Color color)
    {
        Vector2 end = position + direction * 6f;
        DrawLine(position, end, color);
    }
    
    public static void DrawPowerUp(Vector2 position, Color color)
    {
        DrawShape(VectorShapes.PowerUpShape, position, 0, color);
    }

    public static void DrawCompanionDrone(Vector2 position, float rotation)
    {
        Color cyan = Color.Cyan;
        // Draw main diamond body
        DrawShape(VectorShapes.CompanionDroneShape, position, rotation, cyan);
        // Draw fins (not closed polygon)
        DrawShape(VectorShapes.CompanionDroneFins, position, rotation, cyan, closed: false);
    }

    public static void DrawCircle(Vector2 center, float radius, Color color, int segments = 32)
    {
        for (int i = 0; i < segments; i++)
        {
            float angle1 = (float)(i * 2 * Math.PI / segments);
            float angle2 = (float)((i + 1) * 2 * Math.PI / segments);
            
            Vector2 p1 = center + new Vector2(MathF.Cos(angle1), MathF.Sin(angle1)) * radius;
            Vector2 p2 = center + new Vector2(MathF.Cos(angle2), MathF.Sin(angle2)) * radius;
            
            DrawLine(p1, p2, color);
        }
    }
    
    private static Vector2 RotatePoint(Vector2 point, float angle)
    {
        float cos = MathF.Cos(angle);
        float sin = MathF.Sin(angle);
        return new Vector2(
            point.X * cos - point.Y * sin,
            point.X * sin + point.Y * cos
        );
    }
}

