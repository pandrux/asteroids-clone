using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace AsteroidsClone.Audio;

public static class AudioManager
{
    private static ContentManager _content;
    private static Microsoft.Xna.Framework.Audio.SoundEffect[] _soundEffects;
    private static Song _backgroundMusic;
    private static bool _isInitialized;
    private static float _sfxVolume = 0.7f;
    private static float _musicVolume = 0.5f;

    public static void Initialize(ContentManager content)
    {
        _content = content;
        _soundEffects = new Microsoft.Xna.Framework.Audio.SoundEffect[11];
        _isInitialized = true;

        // Load available sound effects
        try
        {
            _soundEffects[(int)SoundEffect.Fire] = content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>("Audio/laserShoot");
            _soundEffects[(int)SoundEffect.Explosion] = content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>("Audio/ShipExplode");
        }
        catch
        {
            // Sound files not found - continue without them
        }

        // Load background music
        try
        {
            _backgroundMusic = content.Load<Song>("Audio/AsteroidMusicLoop");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = _musicVolume;
        }
        catch
        {
            // Music file not found - continue without it
        }
    }
    
    public static void Play(SoundEffect sound)
    {
        if (!_isInitialized) return;

        int index = (int)sound;
        if (index >= 0 && index < _soundEffects.Length && _soundEffects[index] != null)
        {
            _soundEffects[index].Play(_sfxVolume, 0f, 0f);
        }
    }

    public static void Play(SoundEffect sound, float volume, float pitch, float pan)
    {
        if (!_isInitialized) return;

        int index = (int)sound;
        if (index >= 0 && index < _soundEffects.Length && _soundEffects[index] != null)
        {
            _soundEffects[index].Play(volume * _sfxVolume, pitch, pan);
        }
    }

    public static void PlayMusic()
    {
        if (!_isInitialized || _backgroundMusic == null) return;

        if (MediaPlayer.State != MediaState.Playing)
        {
            MediaPlayer.Play(_backgroundMusic);
        }
    }

    public static void StopMusic()
    {
        if (!_isInitialized) return;
        MediaPlayer.Stop();
    }

    public static void PauseMusic()
    {
        if (!_isInitialized) return;
        MediaPlayer.Pause();
    }

    public static void ResumeMusic()
    {
        if (!_isInitialized) return;
        MediaPlayer.Resume();
    }

    public static void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        MediaPlayer.Volume = volume;
    }

    public static void SetSfxVolume(float volume)
    {
        _sfxVolume = volume;
    }
}

