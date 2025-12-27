using System;
using System.Collections.Generic;
using System.Linq;
using AsteroidsClone.Systems.Spawning;

namespace AsteroidsClone.Systems.Wave;

public static class WaveGenerator
{
    private static Random _random = new Random();
    
    public static WaveDefinition GenerateWave(int waveNumber)
    {
        var wave = new WaveDefinition
        {
            WaveNumber = waveNumber,
            AsteroidCount = CalculateAsteroidCount(waveNumber),
            Modifier = GenerateModifier(waveNumber),
            HasBoss = waveNumber % 5 == 0 // Boss every 5 waves
        };
        
        // Add UFOs based on wave number
        wave.UFOs = GenerateUFOs(waveNumber);
        
        return wave;
    }
    
    private static int CalculateAsteroidCount(int waveNumber)
    {
        // Start with 4, add 1 every 2 waves
        return 4 + (waveNumber / 2);
    }
    
    private static List<UFOType> GenerateUFOs(int waveNumber)
    {
        var ufos = new List<UFOType>();

        // Give player a few waves to learn before UFOs appear
        if (waveNumber >= 4)
        {
            // Large UFOs appear from wave 4
            ufos.Add(UFOType.Large);
        }

        if (waveNumber >= 6)
        {
            // Small UFOs appear from wave 6
            ufos.Add(UFOType.Small);
        }

        if (waveNumber >= 8)
        {
            // Hunter UFOs appear from wave 8
            ufos.Add(UFOType.Hunter);
        }

        if (waveNumber >= 10)
        {
            // Bomber UFOs appear from wave 10
            ufos.Add(UFOType.Bomber);
        }

        if (waveNumber >= 12)
        {
            // Shielded UFOs appear from wave 12
            ufos.Add(UFOType.Shielded);
        }

        if (waveNumber >= 14)
        {
            // Phase UFOs appear from wave 14
            ufos.Add(UFOType.Phase);
        }

        if (waveNumber >= 16)
        {
            // Carrier UFOs appear from wave 16
            ufos.Add(UFOType.Carrier);
        }

        return ufos;
    }
    
    private static WaveModifier GenerateModifier(int waveNumber)
    {
        var modifier = new WaveModifier
        {
            AsteroidSpeedMultiplier = 1.0f + (waveNumber * 0.035f),  // Gentler scaling
            UFOFrequency = 1.0f + (waveNumber * 0.055f),             // Slower UFO ramp-up
            PowerUpRate = 1.0f + (waveNumber * 0.05f)
        };
        
        // Special modifiers for certain waves
        if (waveNumber % 3 == 0)
        {
            modifier.Name = "Speed Boost";
            modifier.AsteroidSpeedMultiplier *= 1.5f;
        }
        
        if (waveNumber % 7 == 0)
        {
            modifier.Name = "UFO Swarm";
            modifier.UFOFrequency *= 2.0f;
        }
        
        return modifier;
    }
}


