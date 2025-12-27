using System;
using Microsoft.Xna.Framework;

namespace AsteroidsClone.Systems;

public class PowerSystem
{
    public float MaxPower { get; private set; } = 100f;
    public float CurrentPower { get; set; }
    public float RechargeRate { get; private set; } = 20f; // Per second
    public float ThrustDrainRate { get; private set; } = 15f; // Per second
    public float FireCost { get; private set; } = 10f;
    public float ForceFieldCost { get; private set; } = 30f;
    public float ForceFieldDrainRate { get; private set; } = 25f; // Per second when active
    public float HyperspaceCost { get; private set; } = 50f;
    
    public bool IsForceFieldActive { get; private set; }
    private float _forceFieldTimer;
    private const float ForceFieldDuration = 3.0f;
    
    public PowerSystem()
    {
        CurrentPower = MaxPower;
    }
    
    public void Update(float deltaTime)
    {
        // Recharge power if not at max
        if (CurrentPower < MaxPower && !IsForceFieldActive)
        {
            CurrentPower = Math.Min(MaxPower, CurrentPower + RechargeRate * deltaTime);
        }
        
        // Drain force field if active
        if (IsForceFieldActive)
        {
            CurrentPower -= ForceFieldDrainRate * deltaTime;
            _forceFieldTimer -= deltaTime;
            
            if (CurrentPower <= 0 || _forceFieldTimer <= 0)
            {
                IsForceFieldActive = false;
                CurrentPower = Math.Max(0, CurrentPower);
            }
        }
    }
    
    public float ApplyThrustDrain(float deltaTime)
    {
        if (CurrentPower <= 0) return 0f;
        
        float drain = ThrustDrainRate * deltaTime;
        if (CurrentPower >= drain)
        {
            CurrentPower -= drain;
            return 1.0f; // Full effectiveness
        }
        else
        {
            float effectiveness = CurrentPower / drain;
            CurrentPower = 0;
            return effectiveness; // Reduced effectiveness
        }
    }
    
    public bool CanFire()
    {
        return CurrentPower >= FireCost;
    }
    
    public bool TryFire()
    {
        if (CanFire())
        {
            CurrentPower -= FireCost;
            return true;
        }
        return false;
    }
    
    public bool CanActivateForceField()
    {
        return CurrentPower >= ForceFieldCost && !IsForceFieldActive;
    }
    
    public bool TryActivateForceField()
    {
        if (CanActivateForceField())
        {
            CurrentPower -= ForceFieldCost;
            IsForceFieldActive = true;
            _forceFieldTimer = ForceFieldDuration;
            return true;
        }
        return false;
    }
    
    public void ActivateForceFieldFree()
    {
        // From power-up, no power cost
        IsForceFieldActive = true;
        _forceFieldTimer = ForceFieldDuration;
    }
    
    public bool CanHyperspace()
    {
        return CurrentPower >= HyperspaceCost;
    }
    
    public bool TryHyperspace()
    {
        if (CanHyperspace())
        {
            CurrentPower -= HyperspaceCost;
            return true;
        }
        return false;
    }
    
    public void AddPower(float amount)
    {
        CurrentPower = Math.Min(MaxPower, CurrentPower + amount);
    }
}

