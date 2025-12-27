using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class MainMenu
{
    private SpriteFont _font;
    private float _blinkTimer;
    
    public MainMenu()
    {
        // Font will be loaded from content
    }
    
    public void SetFont(SpriteFont font)
    {
        _font = font;
    }
    
    public void Update(float deltaTime)
    {
        _blinkTimer += deltaTime;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;
        
        // Title
        string title = "ASTEROIDS";
        Vector2 titleSize = _font.MeasureString(title);
        Vector2 titlePos = new Vector2(
            GameState.ScreenWidth / 2 - titleSize.X / 2,
            GameState.ScreenHeight / 2 - 100
        );
        spriteBatch.DrawString(_font, title, titlePos, Color.White);
        
        // Instructions
        string instructions = "Press ENTER to Start";
        Vector2 instSize = _font.MeasureString(instructions);
        Vector2 instPos = new Vector2(
            GameState.ScreenWidth / 2 - instSize.X / 2,
            GameState.ScreenHeight / 2
        );
        
        // Blinking effect
        Color color = ((int)(_blinkTimer * 2) % 2 == 0) ? Color.White : Color.Gray;
        spriteBatch.DrawString(_font, instructions, instPos, color);
        
        // Controls
        string controls = "Arrow Keys: Move | Space: Fire | Shift: Shield | H: Hyperspace";
        Vector2 ctrlSize = _font.MeasureString(controls);
        Vector2 ctrlPos = new Vector2(
            GameState.ScreenWidth / 2 - ctrlSize.X / 2,
            GameState.ScreenHeight / 2 + 50
        );
        spriteBatch.DrawString(_font, controls, ctrlPos, Color.Gray);

        // Settings hint
        string settings = "S - Settings";
        Vector2 settingsSize = _font.MeasureString(settings);
        Vector2 settingsPos = new Vector2(
            GameState.ScreenWidth / 2 - settingsSize.X / 2,
            GameState.ScreenHeight / 2 + 100
        );
        spriteBatch.DrawString(_font, settings, settingsPos, Color.Gray);

        // Leaderboard
        DrawLeaderboard(spriteBatch);
    }

    private void DrawLeaderboard(SpriteBatch spriteBatch)
    {
        float startY = GameState.ScreenHeight / 2 + 150;
        float centerX = GameState.ScreenWidth / 2f;

        // Title
        string title = "HIGH SCORES";
        Vector2 titleSize = _font.MeasureString(title);
        spriteBatch.DrawString(_font, title, new Vector2(centerX - titleSize.X / 2, startY), Color.Yellow);

        // Entries
        var leaderboard = HighScoreManager.Leaderboard;
        for (int i = 0; i < leaderboard.Count && i < 10; i++)
        {
            var entry = leaderboard[i];
            string line = $"{i + 1,2}. {entry.Initials}  {entry.Score,7}";
            Vector2 lineSize = _font.MeasureString(line);
            float y = startY + 30 + (i * 22);
            spriteBatch.DrawString(_font, line, new Vector2(centerX - lineSize.X / 2, y), Color.White);
        }

        // Show placeholder if no scores
        if (leaderboard.Count == 0)
        {
            string noScores = "No scores yet!";
            Vector2 noScoresSize = _font.MeasureString(noScores);
            spriteBatch.DrawString(_font, noScores, new Vector2(centerX - noScoresSize.X / 2, startY + 30), Color.Gray);
        }
    }
}


