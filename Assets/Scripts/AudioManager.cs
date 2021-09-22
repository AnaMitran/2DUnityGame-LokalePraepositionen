using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundAudio;
    [SerializeField] private AudioClip wrongAnswerAudio;
    [SerializeField] private AudioClip correctAnswerAudio;

    private IEnumerator resetVolumeCoroutine;

    private AudioSource audioSourceBackground;
    private AudioSource audioSourceAnswer;

    private float volume = 1f;
    private const float backgroundAudioCoefficient = 1f;
    private const float answerAudioCoefficient = 1f;

    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSourceBackground = audioSources[0];
        audioSourceAnswer = audioSources[1];
        audioSourceBackground.clip = backgroundAudio;
        audioSourceBackground.loop = true;
        audioSourceBackground.Play();
        audioSourceAnswer.Play();
    }

    private void Update()
    {
        
    }
    public void PlayCorrectAnswer()
    {
        if (resetVolumeCoroutine != null)
            StopCoroutine(resetVolumeCoroutine);
        audioSourceAnswer.clip = correctAnswerAudio;
        audioSourceAnswer.Play();
        audioSourceBackground.volume = volume * backgroundAudioCoefficient / 2;
        resetVolumeCoroutine = ResetVolume(correctAnswerAudio.length);
        StartCoroutine(resetVolumeCoroutine);
        
    }
    public void PlayWrongAnswer()
    {
        if (resetVolumeCoroutine != null)
            StopCoroutine(resetVolumeCoroutine);
        audioSourceAnswer.clip = wrongAnswerAudio;
        audioSourceAnswer.Play();
        audioSourceBackground.volume = volume * backgroundAudioCoefficient / 2;
        resetVolumeCoroutine = ResetVolume(wrongAnswerAudio.length);
        StartCoroutine(resetVolumeCoroutine);
    }

    private IEnumerator ResetVolume(float length)
    {
        yield return new WaitForSeconds(length);
        audioSourceBackground.volume = volume * backgroundAudioCoefficient;
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
        audioSourceBackground.volume = volume * backgroundAudioCoefficient;
        audioSourceAnswer.volume = volume * answerAudioCoefficient;
    }
}
