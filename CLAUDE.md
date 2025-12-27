# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build and Run Commands

```bash
# Build the project
dotnet build

# Run the game
dotnet run

# Build release version
dotnet build -c Release
```

## Technology Stack

- **Framework**: MonoGame 3.8 (DesktopGL)
- **Runtime**: .NET 9.0
- **Serialization**: Newtonsoft.Json for config files

## Architecture Overview

### Game Loop (Game1.cs)
The main game class manages state transitions between Menu, Playing, Paused, and GameOver states. The `Update` method dispatches to state-specific handlers, while `Draw` uses `VectorRenderer` for all game graphics.

### Entity System
All game entities inherit from `GameObject` (Entities/GameObject.cs), which provides:
- Position, Velocity, Rotation transforms
- Radius-based collision detection via `CollidesWith()`
- Automatic screen-edge wrapping in `WrapPosition()`
- Entity lifecycle via `IsActive` flag

Entity collections are stored in the static `GameState` class (Core/GameState.cs), which manages:
- Player ship and all entity lists (Asteroids, Bullets, UFOs, PowerUps, Mines, Drones)
- Score tracking with multiplier support
- Entity cleanup via `CleanupInactiveEntities()`

### UFO Hierarchy
UFOs extend `UFOBase` (Entities/UFO/UFOBase.cs) which provides health, firing, and abstract `UpdateAI()`. Specialized types include: SmallUFO, LargeUFO, HunterUFO, BomberUFO, CarrierUFO, PhaseUFO, ShieldedUFO, and BossUFO.

### Core Systems

**CollisionSystem** (Systems/CollisionSystem.cs): Handles all entity collision pairs (player-asteroid, bullet-UFO, etc.) with specific handlers for each collision type.

**PowerSystem** (Systems/PowerSystem.cs): Resource management for ship abilities - thrust, firing, force field, and hyperspace all consume power that recharges over time.

**WaveManager** (Systems/Wave/WaveManager.cs): Controls wave progression, spawning asteroids via `AsteroidSpawner` and UFOs periodically. Wave completion triggers when all asteroids and UFOs are destroyed.

### Rendering
All graphics use `VectorRenderer` (Rendering/VectorRenderer.cs), a line-based rendering system using MonoGame's `BasicEffect`. Shapes are defined in `VectorShapes.cs`. The `Camera` class supports screen shake effects.

### Input
`InputManager` (Core/InputManager.cs) provides static methods for keyboard state:
- `IsPressed()` for single-frame key detection
- `IsHeld()` for continuous input

### Configuration
JSON config files in `/Config` define game parameters (game_config.json, wave_config.json, powerup_config.json, etc.).

## Key Patterns

- Entities are marked inactive (`IsActive = false`) rather than immediately removed; cleanup happens via `GameState.CleanupInactiveEntities()`
- Ship actions (fire, hyperspace, force field) check and consume power via `PowerSystem`
- Asteroids split into smaller sizes when destroyed (Large -> Medium -> Small -> none)
- All drawing is done through `VectorRenderer.Begin()/End()` with line primitives

---

## Game States

```
Menu ←→ Settings
  ↓
Playing ←→ Paused
  ↓
GameOver → [if qualifies] → EnteringInitials → Menu
         → [if not]      → Menu
```

| State | Entry | Exit |
|-------|-------|------|
| **Menu** | Game start, after GameOver/Initials | ENTER→Playing, S→Settings |
| **Settings** | S from Menu | ESC→Menu |
| **Playing** | ENTER from Menu | ESC→Paused, Lives≤0→GameOver |
| **Paused** | ESC from Playing | ESC→Playing |
| **GameOver** | Lives reach 0 | ENTER→EnteringInitials or Menu |
| **EnteringInitials** | Score qualifies for top 10 | Confirm initials→Menu |

---

## Progression Mechanics

### Lives
- Start with 3 lives
- Lose 1 on collision (asteroid, UFO, bullet, mine, drone)
- Gain 1 every 10,000 points
- Game over when lives reach 0

### Respawn
- 1.5 second delay after death
- 2 seconds invulnerability on spawn (cyan glow)
- Enemies won't shoot during invulnerability

### Waves
- Start at Wave 1
- Wave clears when all asteroids and UFOs destroyed
- Asteroid count: 4 + (wave ÷ 2)
- Boss every 5 waves

### UFO Progression
| Wave | UFO Types Available |
|------|---------------------|
| 1-3 | None |
| 4+ | Large |
| 6+ | + Small |
| 8+ | + Hunter |
| 10+ | + Bomber |
| 12+ | + Shielded |
| 14+ | + Phase |
| 16+ | + Carrier |

### Difficulty Scaling (per wave)
- Asteroid speed: +3.5%
- UFO spawn rate: +5.5%
- Power-up rate: +5%

### Special Waves
- Every 3rd wave: "Speed Boost" (1.5x asteroid speed)
- Every 7th wave: "UFO Swarm" (2x UFO frequency)

---

## Player Abilities

| Ability | Key | Power Cost | Notes |
|---------|-----|------------|-------|
| Thrust | ↑ | Continuous | Classic momentum physics |
| Rotate | ←/→ | None | |
| Fire | Space | Per shot | Has cooldown |
| Shield | Shift | Continuous | Absorbs hits |
| Hyperspace | H | One-time | Random teleport, 0.5s invuln after |

Power is a shared pool for thrust, fire, shield, and hyperspace that regenerates over time.

---

## Session Progress (December 26, 2025)

### Completed Features

1. **Font/HUD Text**: Added GameFont.spritefont (Arial Bold 18pt) for HUD display of score, lives, wave, and power bar.

2. **Particle Effects**: Fully implemented ParticleSystem with:
   - Explosion effects (asteroid/UFO destruction)
   - Thrust particles (orange/yellow flames when accelerating)
   - Shield hit sparks (cyan particles on force field impact)
   - Hyperspace teleport effect (purple disappear, magenta appear)
   - Power-up collection sparkles

3. **Ship Death Animation**: Ship breaks into 3 spinning debris line segments that drift apart over 2.5 seconds (via `Debris` struct in ParticleSystem).

4. **Classic Physics**: Removed drag/friction - momentum is fully conserved like original Asteroids. MaxSpeed increased to 600.

5. **Respawn System**: 1.5 second delay after death before respawning. Lives properly decrement.

6. **Spawn Invulnerability**: 2 seconds of invulnerability at game start and after respawn, indicated by cyan glow aura effect (stacked ship outlines) instead of distracting flashing.

7. **Audio System**:
   - Background music loop (AsteroidMusicLoop.mp3) - plays during gameplay, pauses on pause, stops on game over
   - Laser fire sound (laserShoot.wav)
   - Ship explosion sound (ShipExplode.wav)
   - AudioManager handles loading/playback via MonoGame Content Pipeline

### Bug Fixes
- ParticleSystem null reference (moved init to LoadContent)
- HUD texture null (lazy-create white texture)
- Ship orientation (shape now points right, matching angle 0)
- Asteroids not spawning (added GameState.AddEntity call)
- Ship not respawning (added IsActive = true in Respawn)
- Invulnerability flashing in wrong place (moved to Game1.DrawPlaying)
- Wave progression stalemate (waves now advance properly)
- UFOs shooting at invulnerable players (now wait for vulnerability)
- Extra life bug (lives no longer increase on death due to threshold reset)

### Session 2 Features (December 26, 2025)

8. **Screen Shake**: Wired to collision events with toggleable setting:
   - Player death: 0.4s, intensity 15
   - Shield hit: 0.15s, intensity 6
   - Asteroid destruction: varies by size
   - UFO destruction: 0.15s, intensity 5
   - Toggle via Settings menu (S from main menu)

9. **Settings Menu**: New game state accessible from main menu:
   - GameSettings.cs stores preferences
   - SettingsMenu.cs with Up/Down navigation, Enter to toggle

10. **High Score System**: Full arcade-style implementation:
    - Top 10 leaderboard with 3-letter initials
    - All-time high score displayed on HUD (top center, yellow)
    - Leaderboard shown on main menu
    - InitialEntryScreen for entering initials (Up/Down change letter, Left/Right move, Enter confirm)
    - Persisted to highscores.json via HighScoreManager.cs
    - "NEW ALL-TIME HIGH!" and "TOP 10 SCORE!" messages on game over

11. **Difficulty Balancing**:
    - UFOs now appear starting Wave 4 (was Wave 2)
    - Reduced asteroid speed scaling to 3.5%/wave
    - Reduced UFO frequency scaling to 5.5%/wave

### Remaining TODO

1. **More Sound Effects**: The game expects 11 sounds but only 3 are implemented:
   - ShieldActivate, ShieldHit, Hyperspace, PowerEmpty, PowerUpCollect, WaveStart, WaveEnd, MenuNavigate, GameOver
   - Consider replacing music loop with instrumental/chiptune version

2. **Polish**:
   - UFO spawn sounds
   - Wave announcement audio cues
   - Menu music vs gameplay music
