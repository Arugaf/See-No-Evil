using System.Collections;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{
    [Header("������������� ��������������� ��������� ����� �� ������.")]
    private AudioSource source;
    [SerializeField, Tooltip("��������� �����")] private AudioClip[] clips;
    int previousClipIdx;
    [SerializeField] [Range(0.1f, 2)] private float minPitch;
    [SerializeField] [Range(0.1f, 2)] private float maxPitch;

    [SerializeField] [Range(0.001f, 0.1f)] private float offset;

    [SerializeField] private bool _playOnAwake;
    private void Awake()
    {
        if (_playOnAwake)
            PlayRandomSound();
        source = GetComponent<AudioSource>();
        previousClipIdx = Random.Range(0, clips.Length);
    }
    public void PlayRandomSound()
    {
        StartCoroutine(PlayWithOffset());
    }

    IEnumerator PlayWithOffset()
    {
        yield return new WaitForSeconds(Random.Range(0, offset));
        previousClipIdx = (previousClipIdx + Random.Range(1, clips.Length)) % clips.Length;
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(clips[previousClipIdx]);
    }
}

