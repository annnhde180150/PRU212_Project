using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cate : MonoBehaviour
{
    public Animator animator;
    private bool isPlayerNear = false;
    public GameObject shop;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            shop.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isLaying", true);
            animator.SetTrigger("isLayingTrigger");
            isPlayerNear = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isLaying", false);
            animator.SetTrigger("isStandingTrigger");
            isPlayerNear = false;
        }

    }



}