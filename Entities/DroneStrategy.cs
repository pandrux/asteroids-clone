namespace AsteroidsClone.Entities;

public enum PositioningMode
{
    OrbitPlayer,    // Circle around player
    LeadAhead,      // Stay in front of player's movement
    RearGuard,      // Protect from behind
    FlankLeft,      // Left side escort
    FlankRight,     // Right side escort
    Intercept       // Move toward threats
}

public enum TargetingMode
{
    NearestThreat,  // Closest enemy
    LargestFirst,   // Prioritize large asteroids
    UFOPriority,    // Target UFOs first
    ProtectPlayer,  // Target things heading toward player
    ClosingFast     // Target fastest approaching threats
}

public enum BehaviorMode
{
    Aggressive,     // Chase and attack, higher fire rate
    Defensive,      // Stay close, prioritize protection
    Balanced,       // Mix of offense and defense
    Conserve,       // Limited shooting, energy saving
    Evasive         // Avoid damage, opportunistic shots
}

public class DroneStrategy
{
    public PositioningMode Positioning { get; set; } = PositioningMode.OrbitPlayer;
    public TargetingMode Targeting { get; set; } = TargetingMode.NearestThreat;
    public BehaviorMode Behavior { get; set; } = BehaviorMode.Balanced;
}
