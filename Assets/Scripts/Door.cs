using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update

    public void Exit()
    {
        GetComponent<Animator>().SetTrigger("Open");
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        Time.timeScale = .1f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
