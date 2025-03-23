using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private bool isPlayerInDoor = false;
    public string nextScene;
    private void Start()
    {
        animator = GetComponent<Animator>(); 
    }
    private void Update()
    {
        if (isPlayerInDoor && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isOpen", true);
            isPlayerInDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool("isOpen", false);
            isPlayerInDoor = false;
        }
    }
}
