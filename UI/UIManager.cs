using AsteroidsClone.Core;
using Microsoft.Xna.Framework.Graphics;

namespace AsteroidsClone.UI;

public class UIManager
{
    private HUD _hud;
    private MainMenu _mainMenu;
    private SettingsMenu _settingsMenu;
    private PauseMenu _pauseMenu;
    private GameOverScreen _gameOverScreen;
    private WaveAnnouncement _waveAnnouncement;
    private InitialEntryScreen _initialEntryScreen;

    public UIManager()
    {
        _hud = new HUD();
        _mainMenu = new MainMenu();
        _settingsMenu = new SettingsMenu();
        _pauseMenu = new PauseMenu();
        _gameOverScreen = new GameOverScreen();
        _waveAnnouncement = new WaveAnnouncement();
        _initialEntryScreen = new InitialEntryScreen();
    }

    public void SetFont(SpriteFont font)
    {
        _hud.SetFont(font);
        _mainMenu.SetFont(font);
        _settingsMenu.SetFont(font);
        _pauseMenu.SetFont(font);
        _gameOverScreen.SetFont(font);
        _waveAnnouncement.SetFont(font);
        _initialEntryScreen.SetFont(font);
    }
    
    public void Update(float deltaTime, GameStateEnum currentState)
    {
        _waveAnnouncement.Update(deltaTime);
        
        switch (currentState)
        {
            case GameStateEnum.Menu:
                _mainMenu.Update(deltaTime);
                break;
            case GameStateEnum.Settings:
                _settingsMenu.Update(deltaTime);
                break;
            case GameStateEnum.Playing:
                _hud.Update(deltaTime);
                break;
            case GameStateEnum.Paused:
                _pauseMenu.Update(deltaTime);
                break;
            case GameStateEnum.GameOver:
                _gameOverScreen.Update(deltaTime);
                break;
            case GameStateEnum.EnteringInitials:
                _initialEntryScreen.Update(deltaTime);
                break;
        }
    }

    public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, GameStateEnum currentState)
    {
        switch (currentState)
        {
            case GameStateEnum.Menu:
                _mainMenu.Draw(spriteBatch);
                break;
            case GameStateEnum.Settings:
                _settingsMenu.Draw(spriteBatch);
                break;
            case GameStateEnum.Playing:
                _hud.Draw(spriteBatch);
                _waveAnnouncement.Draw(spriteBatch);
                break;
            case GameStateEnum.Paused:
                _hud.Draw(spriteBatch);
                _pauseMenu.Draw(spriteBatch);
                break;
            case GameStateEnum.GameOver:
                _gameOverScreen.Draw(spriteBatch);
                break;
            case GameStateEnum.EnteringInitials:
                _initialEntryScreen.Draw(spriteBatch);
                break;
        }
    }

    public void ShowWaveAnnouncement(int waveNumber)
    {
        _waveAnnouncement.Show(waveNumber);
    }

    public void ResetSettingsMenu()
    {
        _settingsMenu.Reset();
    }

    public GameStateEnum? GetSettingsReturnState()
    {
        return _settingsMenu.GetReturnState();
    }

    public void StartInitialEntry(int score)
    {
        _initialEntryScreen.Reset(score);
    }

    public bool IsInitialEntryComplete()
    {
        return _initialEntryScreen.IsComplete;
    }

    public string GetEnteredInitials()
    {
        return _initialEntryScreen.GetInitials();
    }
}


