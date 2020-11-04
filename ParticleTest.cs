using System;
using UnityEngine;


namespace Assets
{ 
public class ParticleTest : MonoBehaviour
{ 
private ParticleSystem system;

private ParticleSystem.Particle[] particles;

private void Start()
{ 
this.system = base.gameObject.GetComponent<ParticleSystem>();
this.particles = new ParticleSystem.Particle[10000];
}

private void Update()
{ 
for (int i = 0; i < this.particles.Length; i++)
{ 
this.particles[i].position = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * UnityEngine.Random.Range(1f, 40f);
this.particles[i].size = 0.1f;
this.particles[i].color = Color.white;
}
this.system.SetParticles(this.particles, this.particles.Length);
i
}
}
}
