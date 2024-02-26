using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] footstepsClips;
    private AudioSource audioSource;
    private Rigidbody _rigidbody;
    public float footstepThreshold;
    public float footstepRate;
    private float lastFootstepTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (Mathf.Abs(_rigidbody.velocity.y) < 0.1f)
        {
            if(_rigidbody.velocity.magnitude > footstepThreshold)
            {
                if(Time.time - lastFootstepTime > footstepRate)
                {
                    lastFootstepTime = Time.time;
                    audioSource.PlayOneShot(footstepsClips[Random.Range(0, footstepsClips.Length)]);
                }
            }
        }
    }
}
