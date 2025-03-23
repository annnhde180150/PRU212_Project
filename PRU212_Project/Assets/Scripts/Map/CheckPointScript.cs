using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private GameOverScript gameOverScript;
    void Awake()
    {
        //transform.position = PlayerManager.lastCheckPointPos;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("CheckPoint Reached");
            PlayerManager.lastCheckPointPos = transform.position; 
        }
    }

  
}
