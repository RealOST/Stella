using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioManager : PersistentSingleton<AudioManager> {
    [SerializeField] private AudioSource sfxPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<AudioSource> sfxPool = new();

    private const float MIN_PITCH = 0.9f;
    private const float MAX_PITCH = 1.1f;

    protected override void Awake() {
        base.Awake();
        for (var i = 0; i < poolSize; i++) {
            var sfx = Instantiate(sfxPrefab, transform);
            sfx.playOnAwake = false;
            sfxPool.Enqueue(sfx);
        }
    }

    private AudioSource GetAvailableAudioSource() {
        var source = sfxPool.Dequeue();
        sfxPool.Enqueue(source);
        return source;
    }

    public void PlaySFX(AudioData audioData) {
        var source = GetAvailableAudioSource();
        source.pitch = 1f;
        source.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    public void PlaySFX(AudioData audioData, float pitch) {
        var source = GetAvailableAudioSource();
        source.pitch = pitch;
        source.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    public void PlayRandomSFX(AudioData audioData) {
        var source = GetAvailableAudioSource();
        source.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
        source.PlayOneShot(audioData.audioClip, audioData.volume);
    }

    public void PlayRandomSFX(AudioData[] audioData) {
        PlayRandomSFX(audioData[Random.Range(0, audioData.Length)]);
    }

    public AudioClip ReverseAudio(AudioClip originalClip) {
        var samples = new float[originalClip.samples * originalClip.channels];
        originalClip.GetData(samples, 0);

        var reversedSamples = new float[samples.Length];
        var channels = originalClip.channels;

        for (var i = 0; i < originalClip.samples; i++)
        for (var j = 0; j < channels; j++)
            reversedSamples[i * channels + j] = samples[(originalClip.samples - 1 - i) * channels + j];

        var reversedClip = AudioClip.Create(
            originalClip.name + "_reversed",
            originalClip.samples,
            originalClip.channels,
            originalClip.frequency,
            false
        );

        reversedClip.SetData(reversedSamples, 0);
        return reversedClip;
    }
}

[System.Serializable]
public class AudioData {
    public AudioClip audioClip;
    public float volume;
}