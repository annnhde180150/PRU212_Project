using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private bool isPlayerInDoor = false;
    public string nextScene;
    private Vector2 nextScenePos;
    private BossEnemy bossEnemy;

    private Renderer doorRenderer;
    private Collider2D doorCollider;
    private void Start()
    {
        animator = GetComponent<Animator>();

        doorRenderer = GetComponent<Renderer>();
        doorCollider = GetComponent<Collider2D>();

        bossEnemy = FindFirstObjectByType<BossEnemy>();
        if (bossEnemy != null && !bossEnemy.isDead)
        {
            doorRenderer.enabled = false;
            doorCollider.enabled = false;
            Debug.Log("door is closed");
        }
       

    }
    private void Update()
    {
        if (isPlayerInDoor && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {

            if (bossEnemy != null && bossEnemy.isDead)
            {
                GameData gameData = new GameData();
                DataManager.Instance.SaveGame();
            }
            else
            {
                UpdateGameDataForNextScene(nextScene);
            }

            SceneManager.LoadScene(nextScene);
        }

    }
   
    private void UpdateGameDataForNextScene(string nextScene)
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

    public void ShowDoor()
    {
        doorRenderer.enabled = true;
        doorCollider.enabled = true;
        Debug.Log("door is open");
    }


}
