using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Laser : MonoBehaviour
{
    [SerializeField] AudioClip start;
    [SerializeField] AudioClip mid;
    [SerializeField] AudioClip end;
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    public float laserTime;
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
        left = GameObject.Find("leftTarget").transform;
        right = GameObject.Find("rightTarget").transform;
        StartCoroutine(RotateLaser(0.1f,laserTime));
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

    IEnumerator RotateLaser(float rotationDuration, float rotationTimeLimit)
    {
        float totalElapsedTime = 0f;

        while (totalElapsedTime < rotationTimeLimit)
        {
            float elapsedTime = 0f;
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.LookRotation(right.position - transform.position);

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                totalElapsedTime += Time.deltaTime;

                if (totalElapsedTime >= rotationTimeLimit)
                    yield break; // Stop when the time limit is reached

                yield return null;
            }

            // Swap directions
            elapsedTime = 0f;
            startRotation = transform.rotation;
            endRotation = Quaternion.LookRotation(left.position - transform.position);

            while (elapsedTime < rotationDuration)
            {
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
                elapsedTime += Time.deltaTime;
                totalElapsedTime += Time.deltaTime;

                if (totalElapsedTime >= rotationTimeLimit)
                    yield break;

                yield return null;
            }
        }
    }
}
