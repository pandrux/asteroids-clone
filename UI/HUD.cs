using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class HUD
{
    private SpriteFont _font;
    private Texture2D _whiteTexture;

    public HUD()
    {
        // Font will be loaded from content
    }
    
    public void SetFont(SpriteFont font)
    {
        _font = font;
    }
    
    public void Update(float deltaTime)
    {
        // HUD updates if needed
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;
        
        // Score
        string scoreText = $"Score: {GameState.Score}";
        spriteBatch.DrawString(_font, scoreText, new Vector2(10, 10), Color.White);

        // High Score (top center)
        string highScoreText = $"HIGH SCORE: {HighScoreManager.AllTimeHigh}";
        Vector2 highScoreSize = _font.MeasureString(highScoreText);
        Vector2 highScorePos = new Vector2(GameState.ScreenWidth / 2 - highScoreSize.X / 2, 10);
        spriteBatch.DrawString(_font, highScoreText, highScorePos, Color.Yellow);
        
        // Lives
        string livesText = $"Lives: {GameState.Lives}";
        spriteBatch.DrawString(_font, livesText, new Vector2(10, 40), Color.White);
        
        // Wave
        string waveText = $"Wave: {GameState.CurrentWave}";
        spriteBatch.DrawString(_font, waveText, new Vector2(10, 70), Color.White);
        
        // Power bar
        if (GameState.Player != null)
        {
            DrawPowerBar(spriteBatch, new Vector2(10, 100), 200, 20, GameState.Player.Power);
        }
        
        // Active buffs
        if (GameState.Player != null)
        {
            DrawBuffs(spriteBatch, new Vector2(10, 130), GameState.Player.Buffs);
        }
    }
    
    private void DrawPowerBar(SpriteBatch spriteBatch, Vector2 position, float width, float height, Systems.PowerSystem power)
    {
        // Background
        Rectangle bgRect = new Rectangle((int)position.X, (int)position.Y, (int)width, (int)height);
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), bgRect, Color.DarkGray);
        
        // Power fill
        float powerPercent = power.CurrentPower / power.MaxPower;
        Rectangle fillRect = new Rectangle((int)position.X, (int)position.Y, (int)(width * powerPercent), (int)height);
        Color powerColor = powerPercent > 0.5f ? Color.Green : (powerPercent > 0.25f ? Color.Yellow : Color.Red);
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), fillRect, powerColor);
        
        // Border
        DrawRectangle(spriteBatch, bgRect, Color.White, 2);
    }
    
    private void DrawBuffs(SpriteBatch spriteBatch, Vector2 position, Systems.BuffSystem buffs)
    {
        int yOffset = 0;
        foreach (var buff in buffs.GetActiveBuffs())
        {
            string buffText = $"{buff.Name}: {buff.RemainingTime:F1}s";
            spriteBatch.DrawString(_font, buffText, position + new Vector2(0, yOffset), Color.Cyan);
            yOffset += 20;
        }
    }
    
    private Texture2D GetWhiteTexture(SpriteBatch spriteBatch)
    {
        if (_whiteTexture == null)
        {
            _whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });
        }
        return _whiteTexture;
    }
    
    private void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
    {
        // Draw rectangle outline
        // Top
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
        // Bottom
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
        // Left
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
        // Right
        spriteBatch.Draw(GetWhiteTexture(spriteBatch), new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
    }
}


