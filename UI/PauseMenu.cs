using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class PauseMenu
{
    private SpriteFont _font;
    
    public PauseMenu()
    {
        // Font will be loaded from content
    }
    
    public void SetFont(SpriteFont font)
    {
        _font = font;
    }
    
    public void Update(float deltaTime)
    {
        // Pause menu updates if needed
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;
        
        // Semi-transparent overlay
        Rectangle overlay = new Rectangle(0, 0, GameState.ScreenWidth, GameState.ScreenHeight);
        // spriteBatch.Draw(GetWhiteTexture(spriteBatch), overlay, Color.Black * 0.5f);
        
        // Paused text
        string paused = "PAUSED";
        Vector2 pausedSize = _font.MeasureString(paused);
        Vector2 pausedPos = new Vector2(
            GameState.ScreenWidth / 2 - pausedSize.X / 2,
            GameState.ScreenHeight / 2 - 50
        );
        spriteBatch.DrawString(_font, paused, pausedPos, Color.White);
        
        // Instructions
        string instructions = "Press ESC to Resume";
        Vector2 instSize = _font.MeasureString(instructions);
        Vector2 instPos = new Vector2(
            GameState.ScreenWidth / 2 - instSize.X / 2,
            GameState.ScreenHeight / 2 + 20
        );
        spriteBatch.DrawString(_font, instructions, instPos, Color.Gray);
    }
}


