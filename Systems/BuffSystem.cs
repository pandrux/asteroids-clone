using System.Collections.Generic;
using System.Linq;

namespace AsteroidsClone.Systems;

public class Buff
{
    public string Name { get; set; }
    public float Duration { get; set; }
    public float RemainingTime { get; set; }
}

public class BuffSystem
{
    private Dictionary<string, Buff> _activeBuffs = new Dictionary<string, Buff>();
    
    public void Update(float deltaTime)
    {
        var keysToRemove = new List<string>();
        foreach (var kvp in _activeBuffs)
        {
            kvp.Value.RemainingTime -= deltaTime;
            if (kvp.Value.RemainingTime <= 0)
            {
                keysToRemove.Add(kvp.Key);
            }
        }
        
        foreach (var key in keysToRemove)
        {
            _activeBuffs.Remove(key);
        }
    }
    
    public void AddBuff(string name, float duration)
    {
        if (_activeBuffs.ContainsKey(name))
        {
            _activeBuffs[name].RemainingTime = duration;
        }
        else
        {
            _activeBuffs[name] = new Buff { Name = name, Duration = duration, RemainingTime = duration };
        }
    }
    
    public bool HasBuff(string name)
    {
        return _activeBuffs.ContainsKey(name);
    }
    
    public float GetFireCooldownMultiplier()
    {
        if (HasBuff("RapidFire"))
            return 0.5f; // Fire twice as fast
        return 1.0f;
    }
    
    public int GetBulletCount()
    {
        if (HasBuff("MultiShot"))
            return 3; // Triple shot
        return 1;
    }
    
    public bool HasPiercing()
    {
        return HasBuff("Piercing");
    }
    
    public float GetDamageMultiplier()
    {
        if (HasBuff("DamageBoost"))
            return 2.0f;
        return 1.0f;
    }
    
    public float GetSpeedMultiplier()
    {
        if (HasBuff("SpeedBoost"))
            return 1.5f;
        return 1.0f;
    }
    
    public float GetScoreMultiplier()
    {
        if (HasBuff("ScoreMultiplier"))
            return 2.0f;
        return 1.0f;
    }
    
    public IEnumerable<Buff> GetActiveBuffs()
    {
        return _activeBuffs.Values;
    }
}


