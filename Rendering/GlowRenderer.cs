using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.Rendering;

public static class GlowRenderer
{
    private static RenderTarget2D _glowTarget;
    private static SpriteBatch _spriteBatch;
    private static GraphicsDevice _graphicsDevice;
    private static Effect _glowEffect;
    
    public static void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _graphicsDevice = graphicsDevice;
        _spriteBatch = spriteBatch;
        _glowTarget = new RenderTarget2D(
            graphicsDevice,
            graphicsDevice.Viewport.Width,
            graphicsDevice.Viewport.Height
        );
    }
    
    public static void BeginGlowPass()
    {
        _graphicsDevice.SetRenderTarget(_glowTarget);
        _graphicsDevice.Clear(Color.Transparent);
    }
    
    public static void EndGlowPass()
    {
        _graphicsDevice.SetRenderTarget(null);
    }
    
    public static void DrawGlow(float intensity = 1.0f)
    {
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
        _spriteBatch.Draw(_glowTarget, Vector2.Zero, Color.White * intensity);
        _spriteBatch.End();
    }
}


