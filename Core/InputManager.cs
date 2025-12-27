using Microsoft.Xna.Framework.Input;

namespace AsteroidsClone.Core;

public static class InputManager
{
    private static KeyboardState _currentState;
    private static KeyboardState _previousState;
    
    public static void Update()
    {
        _previousState = _currentState;
        _currentState = Keyboard.GetState();
    }
    
    public static bool IsHeld(Keys key) => _currentState.IsKeyDown(key);
    
    public static bool IsPressed(Keys key) => 
        _currentState.IsKeyDown(key) && !_previousState.IsKeyDown(key);
    
    public static bool IsReleased(Keys key) => 
        !_currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
}


