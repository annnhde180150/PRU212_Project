using UnityEngine;

public class TutorialTextTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialText; // Assign the text object in Inspector

    private void Start()
    {
        tutorialText.SetActive(false); // Hide text initially
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.SetActive(true); // Show text when player enters
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialText.SetActive(false); // Hide text when player leaves
        }
    }
}
