using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnFinishParticles : MonoBehaviour {
    private void Update()
    {
        if(GetComponent<ParticleSystem>().isPlaying==false)
        {
            Destroy(this.gameObject);
        }
    }
}
