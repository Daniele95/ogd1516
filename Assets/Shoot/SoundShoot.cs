using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundShoot : MonoBehaviour
{
    private AudioSource shootAudio;

    // Use this for initialization
    void Start()
    {
        shootAudio = GetComponent<AudioSource>();

        shootAudio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
