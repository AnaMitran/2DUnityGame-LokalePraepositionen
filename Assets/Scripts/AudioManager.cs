using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundAudio;
    [SerializeField] private AudioClip wrongAnswerAudio;
    [SerializeField] private AudioClip correctAnswerAudio;

    private IEnumerator switchAudioCorutine;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = backgroundAudio;
        audioSource.Play();
        
    }

    private void Update()
    {
        
    }
    public void PlayCorrectAnswer()
    {
        if (switchAudioCorutine != null)
            StopCoroutine(switchAudioCorutine);
        audioSource.clip = correctAnswerAudio;
        audioSource.Play();
        switchAudioCorutine = SwitchToBackground(correctAnswerAudio.length);
        StartCoroutine(switchAudioCorutine);
    }
    public void PlayWrongAnswer()
    {
        if (switchAudioCorutine != null)
            StopCoroutine(switchAudioCorutine);
        audioSource.clip = wrongAnswerAudio;
        audioSource.Play();
        switchAudioCorutine = SwitchToBackground(wrongAnswerAudio.length);
        StartCoroutine(switchAudioCorutine);
    }

    private IEnumerator SwitchToBackground(float length)
    {
        yield return new WaitForSeconds(length);
        audioSource.clip = backgroundAudio;
        audioSource.Play();
    }
}
