using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class InitialEntryScreen
{
    private SpriteFont _font;
    private char[] _initials = { 'A', 'A', 'A' };
    private int _selectedIndex;
    private int _score;
    private bool _isComplete;
    private float _blinkTimer;

    public InitialEntryScreen()
    {
    }

    public void SetFont(SpriteFont font)
    {
        _font = font;
    }

    public void Reset(int score)
    {
        _initials = new[] { 'A', 'A', 'A' };
        _selectedIndex = 0;
        _score = score;
        _isComplete = false;
        _blinkTimer = 0;
    }

    public bool IsComplete => _isComplete;

    public string GetInitials()
    {
        return new string(_initials);
    }

    public void Update(float deltaTime)
    {
        _blinkTimer += deltaTime;

        // Navigate between letters
        if (InputManager.IsPressed(Keys.Left))
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = 2;
        }
        if (InputManager.IsPressed(Keys.Right))
        {
            _selectedIndex++;
            if (_selectedIndex > 2) _selectedIndex = 0;
        }

        // Change letter
        if (InputManager.IsPressed(Keys.Up))
        {
            _initials[_selectedIndex]++;
            if (_initials[_selectedIndex] > 'Z') _initials[_selectedIndex] = 'A';
        }
        if (InputManager.IsPressed(Keys.Down))
        {
            _initials[_selectedIndex]--;
            if (_initials[_selectedIndex] < 'A') _initials[_selectedIndex] = 'Z';
        }

        // Confirm
        if (InputManager.IsPressed(Keys.Enter))
        {
            _isComplete = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;

        float centerX = GameState.ScreenWidth / 2f;
        float centerY = GameState.ScreenHeight / 2f;

        // Title
        string title = "NEW HIGH SCORE!";
        Vector2 titleSize = _font.MeasureString(title);
        Vector2 titlePos = new Vector2(centerX - titleSize.X / 2, centerY - 120);
        spriteBatch.DrawString(_font, title, titlePos, Color.Yellow);

        // Score
        string scoreText = _score.ToString();
        Vector2 scoreSize = _font.MeasureString(scoreText);
        Vector2 scorePos = new Vector2(centerX - scoreSize.X / 2, centerY - 80);
        spriteBatch.DrawString(_font, scoreText, scorePos, Color.White);

        // Prompt
        string prompt = "Enter Your Initials:";
        Vector2 promptSize = _font.MeasureString(prompt);
        Vector2 promptPos = new Vector2(centerX - promptSize.X / 2, centerY - 20);
        spriteBatch.DrawString(_font, prompt, promptPos, Color.Gray);

        // Draw initials with brackets
        float letterSpacing = 50f;
        float startX = centerX - letterSpacing;

        for (int i = 0; i < 3; i++)
        {
            bool isSelected = i == _selectedIndex;
            string letterText = $"[ {_initials[i]} ]";
            Vector2 letterSize = _font.MeasureString(letterText);
            float x = startX + (i * letterSpacing) - letterSize.X / 2;
            float y = centerY + 20;

            // Blink selected letter
            Color color;
            if (isSelected)
            {
                color = ((int)(_blinkTimer * 4) % 2 == 0) ? Color.Yellow : Color.White;
            }
            else
            {
                color = Color.White;
            }

            spriteBatch.DrawString(_font, letterText, new Vector2(x, y), color);
        }

        // Instructions
        string inst1 = "UP/DOWN: Change Letter";
        Vector2 inst1Size = _font.MeasureString(inst1);
        spriteBatch.DrawString(_font, inst1, new Vector2(centerX - inst1Size.X / 2, centerY + 80), Color.Gray);

        string inst2 = "LEFT/RIGHT: Move";
        Vector2 inst2Size = _font.MeasureString(inst2);
        spriteBatch.DrawString(_font, inst2, new Vector2(centerX - inst2Size.X / 2, centerY + 110), Color.Gray);

        string inst3 = "ENTER: Confirm";
        Vector2 inst3Size = _font.MeasureString(inst3);
        spriteBatch.DrawString(_font, inst3, new Vector2(centerX - inst3Size.X / 2, centerY + 140), Color.Gray);
    }
}
