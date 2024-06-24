using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] GameObject[] particles;
    public static ParticleManager instance;

    void Awake() {
        instance = this;
    }

    public void SpawnParticle(int id, Vector3 position, Quaternion direction) {
        GameObject part = Instantiate(particles[id],position,direction);
        Destroy(part, part.GetComponent<ParticleSystem>().main.duration);
    }
}
