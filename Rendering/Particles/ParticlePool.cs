using System.Collections.Generic;

namespace AsteroidsClone.Rendering.Particles;

public class ParticlePool
{
    private readonly Queue<Particle> _availableParticles;
    private readonly int _maxParticles;
    
    public ParticlePool(int maxParticles)
    {
        _maxParticles = maxParticles;
        _availableParticles = new Queue<Particle>();
        
        // Pre-allocate particles
        for (int i = 0; i < maxParticles; i++)
        {
            _availableParticles.Enqueue(new Particle());
        }
    }
    
    public Particle Get()
    {
        if (_availableParticles.Count > 0)
        {
            return _availableParticles.Dequeue();
        }
        return new Particle(); // Create new if pool is empty
    }
    
    public void Return(Particle particle)
    {
        if (_availableParticles.Count < _maxParticles)
        {
            particle.IsActive = false;
            _availableParticles.Enqueue(particle);
        }
    }
}


