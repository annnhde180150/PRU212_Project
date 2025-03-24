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
        GetComponent<BoxCollider2D>().enabled = false;
        var player = GameObject.Find("Player").transform.position;
        audio.PlayOneShot(start);

        StartCoroutine(Lasering());
        left = GameObject.Find("leftTarget").transform;
        right = GameObject.Find("rightTarget").transform;
        StartCoroutine(RotateLaser(laserTime, left, right,right.position.x - player.x < left.position.x - player.x));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Lasering()
    {
        yield return new WaitForSeconds(stateInfo.length);
        GetComponent<BoxCollider2D>().enabled = true;
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

    IEnumerator RotateLaser(float rotationDuration, Transform left, Transform right, bool rotateRight)
    {
        float elapsedTime = 0f;
        float startAngle, endAngle;

        if (rotateRight)
        {
            startAngle = Mathf.Atan2(left.position.y - transform.position.y, left.position.x - transform.position.x) * Mathf.Rad2Deg;
            endAngle = Mathf.Atan2(right.position.y - transform.position.y, right.position.x - transform.position.x) * Mathf.Rad2Deg;
        }
        else
        {
            startAngle = Mathf.Atan2(right.position.y - transform.position.y, right.position.x - transform.position.x) * Mathf.Rad2Deg;
            endAngle = Mathf.Atan2(left.position.y - transform.position.y, left.position.x - transform.position.x) * Mathf.Rad2Deg;
        }

        while (elapsedTime < rotationDuration)
        {
            float zRotation = Mathf.Lerp(startAngle, endAngle, elapsedTime / rotationDuration);
            transform.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, endAngle);
    }



}
