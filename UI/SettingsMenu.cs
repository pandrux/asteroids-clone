using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidsClone.Core;

namespace AsteroidsClone.UI;

public class SettingsMenu
{
    private SpriteFont _font;
    private int _selectedIndex;
    private GameStateEnum? _returnState;

    private const int OptionCount = 2; // Screen Shake, Back

    public SettingsMenu()
    {
    }

    public void SetFont(SpriteFont font)
    {
        _font = font;
    }

    public void Reset()
    {
        _selectedIndex = 0;
        _returnState = null;
    }

    public GameStateEnum? GetReturnState()
    {
        var state = _returnState;
        _returnState = null;
        return state;
    }

    public void Update(float deltaTime)
    {
        // Navigation
        if (InputManager.IsPressed(Keys.Up))
        {
            _selectedIndex--;
            if (_selectedIndex < 0) _selectedIndex = OptionCount - 1;
        }
        if (InputManager.IsPressed(Keys.Down))
        {
            _selectedIndex++;
            if (_selectedIndex >= OptionCount) _selectedIndex = 0;
        }

        // Toggle/Select
        if (InputManager.IsPressed(Keys.Enter) ||
            InputManager.IsPressed(Keys.Left) ||
            InputManager.IsPressed(Keys.Right))
        {
            HandleSelection();
        }

        // Return to menu
        if (InputManager.IsPressed(Keys.Escape))
        {
            _returnState = GameStateEnum.Menu;
        }
    }

    private void HandleSelection()
    {
        switch (_selectedIndex)
        {
            case 0: // Screen Shake
                GameSettings.ScreenShakeEnabled = !GameSettings.ScreenShakeEnabled;
                break;
            case 1: // Back
                _returnState = GameStateEnum.Menu;
                break;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_font == null) return;

        // Title
        string title = "SETTINGS";
        Vector2 titleSize = _font.MeasureString(title);
        Vector2 titlePos = new Vector2(
            GameState.ScreenWidth / 2 - titleSize.X / 2,
            GameState.ScreenHeight / 2 - 120
        );
        spriteBatch.DrawString(_font, title, titlePos, Color.White);

        // Screen Shake option
        string shakeText = $"Screen Shake: {(GameSettings.ScreenShakeEnabled ? "ON" : "OFF")}";
        DrawOption(spriteBatch, shakeText, 0, GameState.ScreenHeight / 2 - 30);

        // Back option
        DrawOption(spriteBatch, "Back", 1, GameState.ScreenHeight / 2 + 20);

        // Instructions
        string instructions = "UP/DOWN: Navigate | ENTER: Select | ESC: Back";
        Vector2 instSize = _font.MeasureString(instructions);
        Vector2 instPos = new Vector2(
            GameState.ScreenWidth / 2 - instSize.X / 2,
            GameState.ScreenHeight / 2 + 100
        );
        spriteBatch.DrawString(_font, instructions, instPos, Color.Gray);
    }

    private void DrawOption(SpriteBatch spriteBatch, string text, int index, float y)
    {
        bool isSelected = index == _selectedIndex;
        string prefix = isSelected ? "> " : "  ";
        string fullText = prefix + text;

        Vector2 size = _font.MeasureString(fullText);
        Vector2 pos = new Vector2(
            GameState.ScreenWidth / 2 - size.X / 2,
            y
        );

        Color color = isSelected ? Color.Yellow : Color.White;
        spriteBatch.DrawString(_font, fullText, pos, color);
    }
}
