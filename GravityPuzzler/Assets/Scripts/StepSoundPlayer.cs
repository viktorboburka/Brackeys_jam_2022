using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StepSoundPlayer : MonoBehaviour {

    [SerializeField]
    AudioClip[] _clips;

    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // called from animation
    public void Play()
    {
        if (_clips == null) return;
        if (Level.activeLevel.state != Level.State.RUNNING) return;

        var clip = _clips[Random.Range(0, _clips.Length)];
        source.PlayOneShot(clip);
    }
}
