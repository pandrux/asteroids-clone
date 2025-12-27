using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AsteroidsClone.Core;
using AsteroidsClone.Systems;

namespace AsteroidsClone;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    // Managers
    private CollisionSystem _collisionSystem;
    private Systems.Wave.WaveManager _waveManager;
    private UI.UIManager _uiManager;
    private Rendering.Camera _camera;
    private Rendering.Particles.ParticleSystem _particleSystem;
    
    // Game state
    private GameStateEnum _currentState = GameStateEnum.Menu;

    // Respawn delay
    private float _respawnTimer = 0f;
    private const float RespawnDelay = 1.5f;
    private bool _waitingToRespawn = false;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    
    protected override void Initialize()
    {
        // Set up 1280x720 window
        _graphics.PreferredBackBufferWidth = GameState.ScreenWidth;
        _graphics.PreferredBackBufferHeight = GameState.ScreenHeight;
        _graphics.ApplyChanges();
        
        // Fixed timestep at 60 FPS
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
        
        // Initialize systems
        InputManager.Update();
        _collisionSystem = new CollisionSystem();
        _waveManager = new Systems.Wave.WaveManager();
        _uiManager = new UI.UIManager();
        _camera = new Rendering.Camera();
        _collisionSystem.SetCamera(_camera);
        Audio.AudioManager.Initialize(Content);
        HighScoreManager.Load();
        
        // Initialize vector renderer
        Rendering.VectorRenderer.Initialize(GraphicsDevice);
        
        base.Initialize();
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Initialize particle system (needs SpriteBatch)
        _particleSystem = new Rendering.Particles.ParticleSystem(_spriteBatch, GraphicsDevice);

        // Load font
        SpriteFont font = Content.Load<SpriteFont>("Fonts/GameFont");
        _uiManager.SetFont(font);
    }
    
    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        InputManager.Update();
        
        // Update camera (for screen shake)
        _camera.Update(deltaTime);
        
        // Update particle system
        _particleSystem.Update(deltaTime);
        
        _uiManager.Update(deltaTime, _currentState);
        
        switch (_currentState)
        {
            case GameStateEnum.Menu:
                UpdateMenu(deltaTime);
                break;
            case GameStateEnum.Settings:
                UpdateSettings(deltaTime);
                break;
            case GameStateEnum.Playing:
                UpdatePlaying(deltaTime);
                break;
            case GameStateEnum.Paused:
                UpdatePaused(deltaTime);
                break;
            case GameStateEnum.GameOver:
                UpdateGameOver(deltaTime);
                break;
            case GameStateEnum.EnteringInitials:
                UpdateEnteringInitials(deltaTime);
                break;
        }

        base.Update(gameTime);
    }
    
    private void UpdateMenu(float deltaTime)
    {
        if (InputManager.IsPressed(Keys.Enter))
        {
            StartNewGame();
            _currentState = GameStateEnum.Playing;
        }
        if (InputManager.IsPressed(Keys.S))
        {
            _uiManager.ResetSettingsMenu();
            _currentState = GameStateEnum.Settings;
        }
    }

    private void UpdateSettings(float deltaTime)
    {
        var returnState = _uiManager.GetSettingsReturnState();
        if (returnState.HasValue)
        {
            _currentState = returnState.Value;
        }
    }
    
    private void UpdatePlaying(float deltaTime)
    {
        // Update wave manager
        _waveManager.Update(deltaTime);
        
        // Check if wave is complete and start next wave
        if (_waveManager.IsWaveComplete)
        {
            GameState.CurrentWave++;
            _waveManager.StartWave(GameState.CurrentWave);
            _uiManager.ShowWaveAnnouncement(GameState.CurrentWave);
        }
        
        // Update player
        if (GameState.Player != null && GameState.Player.IsActive)
        {
            GameState.Player.Update(deltaTime);
        }

        // Handle respawn delay
        if (GameState.Player != null && !GameState.Player.IsActive && GameState.Lives > 0)
        {
            if (!_waitingToRespawn)
            {
                _waitingToRespawn = true;
                _respawnTimer = RespawnDelay;
            }
            else
            {
                _respawnTimer -= deltaTime;
                if (_respawnTimer <= 0)
                {
                    GameState.Player.Respawn(new Vector2(GameState.ScreenWidth / 2, GameState.ScreenHeight / 2));
                    _waitingToRespawn = false;
                }
            }
        }

        // Update asteroids
        foreach (var asteroid in GameState.Asteroids)
        {
            asteroid.Update(deltaTime);
        }
        
        // Update bullets
        foreach (var bullet in GameState.PlayerBullets)
        {
            bullet.Update(deltaTime);
        }
        foreach (var bullet in GameState.EnemyBullets)
        {
            bullet.Update(deltaTime);
        }
        
        // Update UFOs
        foreach (var ufo in GameState.UFOs)
        {
            ufo.Update(deltaTime);
        }
        
        // Update power-ups
        foreach (var powerUp in GameState.PowerUps)
        {
            powerUp.Update(deltaTime);
        }
        
        // Update mines
        foreach (var mine in GameState.Mines)
        {
            mine.Update(deltaTime);
        }
        
        // Update drones
        foreach (var drone in GameState.Drones)
        {
            drone.Update(deltaTime);
        }
        
        // Collision detection
        _collisionSystem.Update();
        
        // Cleanup inactive entities
        GameState.CleanupInactiveEntities();
        
        // Pause
        if (InputManager.IsPressed(Keys.Escape))
        {
            _currentState = GameStateEnum.Paused;
            Audio.AudioManager.PauseMusic();
        }

        // Game over check
        if (GameState.Lives <= 0)
        {
            _currentState = GameStateEnum.GameOver;
            Audio.AudioManager.StopMusic();
        }
    }
    
    private void UpdatePaused(float deltaTime)
    {
        if (InputManager.IsPressed(Keys.Escape))
        {
            _currentState = GameStateEnum.Playing;
            Audio.AudioManager.ResumeMusic();
        }
    }
    
    private void UpdateGameOver(float deltaTime)
    {
        if (InputManager.IsPressed(Keys.Enter))
        {
            if (HighScoreManager.QualifiesForLeaderboard(GameState.Score))
            {
                _uiManager.StartInitialEntry(GameState.Score);
                _currentState = GameStateEnum.EnteringInitials;
            }
            else
            {
                _currentState = GameStateEnum.Menu;
            }
        }
    }

    private void UpdateEnteringInitials(float deltaTime)
    {
        if (_uiManager.IsInitialEntryComplete())
        {
            string initials = _uiManager.GetEnteredInitials();
            HighScoreManager.AddEntry(initials, GameState.Score);
            _currentState = GameStateEnum.Menu;
        }
    }
    
    private void StartNewGame()
    {
        GameState.Reset();
        GameState.Player = new Entities.Ship();
        // Give player initial spawn invulnerability (like classic Asteroids)
        GameState.Player.IsInvulnerable = true;
        GameState.Player.InvulnerabilityTimer = 2.0f;
        GameState.CurrentWave = 1;
        _waveManager.Reset();
        _waveManager.StartWave(GameState.CurrentWave);
        _waitingToRespawn = false;
        _respawnTimer = 0f;

        // Start background music
        Audio.AudioManager.PlayMusic();
    }
    
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        
        // Draw game world with vector graphics
        Rendering.VectorRenderer.Begin(_camera);
        
        switch (_currentState)
        {
            case GameStateEnum.Menu:
            case GameStateEnum.Settings:
                // Menu and Settings are drawn by UI system
                break;
            case GameStateEnum.Playing:
            case GameStateEnum.Paused:
                DrawPlaying();
                break;
            case GameStateEnum.GameOver:
            case GameStateEnum.EnteringInitials:
                // Game over and initial entry screens are drawn by UI system
                break;
        }

        // Draw debris (ship death effect) - needs to be within VectorRenderer Begin/End
        _particleSystem.DrawDebris();

        Rendering.VectorRenderer.End();
        
        // Draw particles
        _particleSystem.Draw();
        
        // Draw UI
        _spriteBatch.Begin();
        _uiManager.Draw(_spriteBatch, _currentState);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
    
    private void DrawPlaying()
    {
        // Draw player
        if (GameState.Player != null && GameState.Player.IsActive)
        {
            Rendering.VectorRenderer.DrawShip(
                GameState.Player.Position,
                GameState.Player.Rotation,
                GameState.Player.IsThrusting,
                GameState.Player.IsShielded,
                GameState.Player.IsInvulnerable
            );
        }

        // Draw asteroids
        foreach (var asteroid in GameState.Asteroids)
        {
            Rendering.VectorRenderer.DrawAsteroid(
                asteroid.Position,
                asteroid.Rotation,
                asteroid.Radius,
                Color.White
            );
        }
        
        // Draw bullets
        foreach (var bullet in GameState.PlayerBullets)
        {
            Rendering.VectorRenderer.DrawBullet(
                bullet.Position,
                Vector2.Normalize(bullet.Velocity),
                Color.White
            );
        }
        foreach (var bullet in GameState.EnemyBullets)
        {
            Rendering.VectorRenderer.DrawBullet(
                bullet.Position,
                Vector2.Normalize(bullet.Velocity),
                Color.Red
            );
        }
        
        // Draw UFOs
        foreach (var ufo in GameState.UFOs)
        {
            Rendering.VectorRenderer.DrawUFO(
                ufo.Position,
                ufo.Radius,
                Color.Orange
            );
        }
        
        // Draw power-ups
        foreach (var powerUp in GameState.PowerUps)
        {
            Rendering.VectorRenderer.DrawPowerUp(
                powerUp.Position,
                Color.Gold
            );
        }
        
        // Draw mines
        foreach (var mine in GameState.Mines)
        {
            Rendering.VectorRenderer.DrawCircle(
                mine.Position,
                mine.Radius,
                Color.Orange,
                16
            );
        }
        
        // Draw drones
        foreach (var drone in GameState.Drones)
        {
            Rendering.VectorRenderer.DrawCircle(
                drone.Position,
                drone.Radius,
                Color.Red,
                12
            );
        }
    }
}
