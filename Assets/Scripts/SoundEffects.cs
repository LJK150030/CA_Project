using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioClip Sound;

    private AudioSource _effect;

    private void Start()
    {
        _effect = GetComponent<AudioSource>();
        _effect.playOnAwake = false;
        _effect.clip = Sound;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.CompareTag("Ball"))
            _effect.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Ball"))
            _effect.Play();
    }
}
