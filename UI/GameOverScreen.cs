using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class GameOverScreen
{
    private SpriteFont _font;
    private float _blinkTimer;
    
    public GameOverScreen()
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
        
        // Game Over text
        string gameOver = "GAME OVER";
        Vector2 goSize = _font.MeasureString(gameOver);
        Vector2 goPos = new Vector2(
            GameState.ScreenWidth / 2 - goSize.X / 2,
            GameState.ScreenHeight / 2 - 100
        );
        spriteBatch.DrawString(_font, gameOver, goPos, Color.Red);
        
        // Final score
        string scoreText = $"Final Score: {GameState.Score}";
        Vector2 scoreSize = _font.MeasureString(scoreText);
        Vector2 scorePos = new Vector2(
            GameState.ScreenWidth / 2 - scoreSize.X / 2,
            GameState.ScreenHeight / 2 - 50
        );
        spriteBatch.DrawString(_font, scoreText, scorePos, Color.White);
        
        // Wave reached
        string waveText = $"Waves Survived: {GameState.CurrentWave}";
        Vector2 waveSize = _font.MeasureString(waveText);
        Vector2 wavePos = new Vector2(
            GameState.ScreenWidth / 2 - waveSize.X / 2,
            GameState.ScreenHeight / 2
        );
        spriteBatch.DrawString(_font, waveText, wavePos, Color.White);

        // High score achievement message
        if (GameState.Score > HighScoreManager.AllTimeHigh && GameState.Score > 0)
        {
            string newHigh = "NEW ALL-TIME HIGH!";
            Vector2 newHighSize = _font.MeasureString(newHigh);
            Vector2 newHighPos = new Vector2(
                GameState.ScreenWidth / 2 - newHighSize.X / 2,
                GameState.ScreenHeight / 2 + 40
            );
            Color flashColor = ((int)(_blinkTimer * 4) % 2 == 0) ? Color.Yellow : Color.Gold;
            spriteBatch.DrawString(_font, newHigh, newHighPos, flashColor);
        }
        else if (HighScoreManager.QualifiesForLeaderboard(GameState.Score))
        {
            string topTen = "TOP 10 SCORE!";
            Vector2 topTenSize = _font.MeasureString(topTen);
            Vector2 topTenPos = new Vector2(
                GameState.ScreenWidth / 2 - topTenSize.X / 2,
                GameState.ScreenHeight / 2 + 40
            );
            spriteBatch.DrawString(_font, topTen, topTenPos, Color.Cyan);
        }

        // Instructions
        string instructions = "Press ENTER to Continue";
        Vector2 instSize = _font.MeasureString(instructions);
        Vector2 instPos = new Vector2(
            GameState.ScreenWidth / 2 - instSize.X / 2,
            GameState.ScreenHeight / 2 + 80
        );
        
        // Blinking effect
        Color color = ((int)(_blinkTimer * 2) % 2 == 0) ? Color.White : Color.Gray;
        spriteBatch.DrawString(_font, instructions, instPos, color);
    }
}


