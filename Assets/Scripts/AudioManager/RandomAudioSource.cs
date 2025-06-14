using Features.AudioManager;
using System.Collections;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{
    [SerializeField] private AudioStepMaterial randomParameters;
    [SerializeField] private bool _playOnStart;
    private void Start()
    {
        if (_playOnStart) PlayRandomSound();
    }
    public void PlayRandomSound()
    {
        AudioManager.PlayAtomic(transform.position, randomParameters.Generate());
    }
}

