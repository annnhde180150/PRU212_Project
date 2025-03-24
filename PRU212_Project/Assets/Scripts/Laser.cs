using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Laser : MonoBehaviour
{
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip mid;
    [SerializeField] AudioClip end;
    AudioSource audio;
    Animator animation;
    AnimatorStateInfo stateInfo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audio = GetComponent<AudioSource>();
        animation = GetComponent<Animator>();
        stateInfo = animation.GetCurrentAnimatorStateInfo(0);
        audio.PlayOneShot(start);
        StartCoroutine(Lasering());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Lasering()
    {
        yield return new WaitForSeconds(stateInfo.length);
        audio.clip = mid;
        audio.loop = true;  // Ensure looping
        audio.Play();
        animation.SetBool("IsLasering",true);
    }

    public IEnumerator End()
    {
        audio.Stop();
        audio.loop=false;
        audio.PlayOneShot(end);
        yield return new WaitForSeconds(end.length);
        Destroy(gameObject);
    }
}
