using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
    int sceneIndex = 0;

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
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex != currentScene)
            Destroy(gameObject);
    }
}
