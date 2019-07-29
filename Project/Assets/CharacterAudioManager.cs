using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudioManager : MonoBehaviour
{
    public AudioClip[] FootstepSounds;
    public float FootstepTime;
    public float FootstepPitchVariation;

    protected bool IsMoving = false;
    protected float FootstepTimer = 0;
    protected AudioSource Source = null;

    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        FootstepTimer += Time.deltaTime;
        if(IsMoving && FootstepTimer > FootstepTime)
        {
            FootstepTimer = 0.0f;
            Source.clip = FootstepSounds[Random.Range(0, FootstepSounds.Length)];
            Source.pitch = 1 + Random.Range(-FootstepPitchVariation, FootstepPitchVariation);
            Source.Play();
        }
    }

    public void SetMoving(bool moving)
    {
        IsMoving = moving;
    }

    public bool GetMoving()
    {
        return IsMoving;
    }
}
