using System;
using Microsoft.Xna.Framework;
using AsteroidsClone.Core;
using AsteroidsClone.Systems.Spawning;

namespace AsteroidsClone.Systems.Wave;

public class WaveManager
{
    private WaveDefinition _currentWave;
    private float _waveStartTimer;
    private float _ufoSpawnTimer;
    private float _nextUFOSpawn;
    private Random _random = new Random();
    private bool _waveInProgress;
    
    public WaveDefinition CurrentWave => _currentWave;
    public bool IsWaveComplete => _waveInProgress && 
        GameState.Asteroids.Count == 0 && 
        GameState.UFOs.Count == 0 &&
        !GameState.HasActiveBoss;
    
    public void StartWave(int waveNumber)
    {
        _currentWave = WaveGenerator.GenerateWave(waveNumber);
        _waveInProgress = true;
        _waveStartTimer = 0;
        _ufoSpawnTimer = 0;
        _nextUFOSpawn = CalculateNextUFOSpawn();
        
        // Spawn initial asteroids
        AsteroidSpawner.SpawnWave(waveNumber, _currentWave.AsteroidCount);
        
        // Spawn boss if this wave has one
        if (_currentWave.HasBoss)
        {
            var boss = UFOSpawner.SpawnUFO(UFOType.Boss);
            GameState.AddEntity(boss);
        }
    }
    
    public void Update(float deltaTime)
    {
        if (!_waveInProgress) return;
        
        _waveStartTimer += deltaTime;
        _ufoSpawnTimer += deltaTime;
        
        // Spawn UFOs periodically
        if (_ufoSpawnTimer >= _nextUFOSpawn && _currentWave.UFOs.Count > 0)
        {
            SpawnRandomUFO();
            _ufoSpawnTimer = 0;
            _nextUFOSpawn = CalculateNextUFOSpawn();
        }
        
        // Wave completion is checked by Game1 via IsWaveComplete property
        // StartWave() will reset state when next wave begins
    }
    
    private void SpawnRandomUFO()
    {
        if (_currentWave.UFOs.Count == 0) return;
        
        UFOType type = _currentWave.UFOs[_random.Next(_currentWave.UFOs.Count)];
        var ufo = UFOSpawner.SpawnUFO(type);
        GameState.AddEntity(ufo);
    }
    
    private float CalculateNextUFOSpawn()
    {
        // Base spawn rate modified by wave difficulty
        float baseRate = 10.0f;
        float modifier = _currentWave?.Modifier?.UFOFrequency ?? 1.0f;
        return baseRate / modifier;
    }
    
    public void Reset()
    {
        _waveInProgress = false;
        _currentWave = null;
        _waveStartTimer = 0;
        _ufoSpawnTimer = 0;
    }
}


