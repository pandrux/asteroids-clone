using System;

namespace AsteroidsClone.Systems.Wave;

public static class DifficultyScaler
{
    public static float GetDifficultyMultiplier(int waveNumber)
    {
        // Exponential difficulty scaling
        return 1.0f + (waveNumber * 0.1f);
    }
    
    public static float GetAsteroidSpeedMultiplier(int waveNumber)
    {
        return 1.0f + (waveNumber * 0.05f);
    }
    
    public static float GetUFOSpawnRate(int waveNumber)
    {
        // UFOs spawn more frequently as waves progress
        return Math.Min(1.0f, 0.1f + (waveNumber * 0.02f));
    }
}

