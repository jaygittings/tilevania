using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Session : MonoBehaviour
{
    [SerializeField] int maxPlayerLives = 3;

    int currentPlayerLives;

    //Singleton pattern
    private void Awake()
    {
        //Singleton pattern for game object
        int objCount = FindObjectsOfType(GetType()).Length;
        if (objCount > 1)
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerLives = maxPlayerLives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerDied()
    {
        currentPlayerLives--;

        if(currentPlayerLives <= 0)
        {
            //restart game
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
        else
        {
            //restart scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
