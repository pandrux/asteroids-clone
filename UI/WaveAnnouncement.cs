using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class WaveAnnouncement
{
    private SpriteFont _font;
    private bool _isShowing;
    private float _displayTimer;
    private int _currentWave;
    private const float DisplayDuration = 3.0f;
    
    public WaveAnnouncement()
    {
        // Font will be loaded from content
    }
    
    public void SetFont(SpriteFont font)
    {
        _font = font;
    }
    
    public void Show(int waveNumber)
    {
        _currentWave = waveNumber;
        _isShowing = true;
        _displayTimer = DisplayDuration;
    }
    
    public void Update(float deltaTime)
    {
        if (_isShowing)
        {
            _displayTimer -= deltaTime;
            if (_displayTimer <= 0)
            {
                _isShowing = false;
            }
        }
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        if (!_isShowing || _font == null) return;
        
        // Fade out effect
        float alpha = MathHelper.Clamp(_displayTimer / DisplayDuration, 0f, 1f);
        Color color = Color.White * alpha;
        
        string text = $"WAVE {_currentWave}";
        Vector2 textSize = _font.MeasureString(text);
        Vector2 textPos = new Vector2(
            GameState.ScreenWidth / 2 - textSize.X / 2,
            GameState.ScreenHeight / 2 - textSize.Y / 2
        );
        
        spriteBatch.DrawString(_font, text, textPos, color);
    }
}


