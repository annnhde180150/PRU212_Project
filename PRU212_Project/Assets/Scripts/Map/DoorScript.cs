using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private bool isPlayerInDoor = false;
    public string nextScene;
    private Vector2 nextScenePos = new Vector2(-9.7f, -3.6f);
    private void Start()
    {
        animator = GetComponent<Animator>(); 
    }
    private void Update()
    {
        if (isPlayerInDoor && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            UpdateGameDataForNextScene();
            SceneManager.LoadScene(nextScene);
        }
    }
   
    private void UpdateGameDataForNextScene()
    {
        GameData gameData = DataManager.Instance.GetGameData();

        PlayerManager.lastCheckPointPos = nextScenePos;
        gameData.currentLevel = nextScene;
        //gameData.checkpointPos = nextScenePos;

        Debug.Log("Saving Game Data for next scene: " + gameData.currentLevel + " at position: " + gameData.checkpointPos);
        DataManager.Instance.SaveGame();
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
