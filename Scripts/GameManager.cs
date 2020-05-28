using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject replayButton;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void GameOver()
    {
        StartCoroutine(ExampleCoroutine());
        //Time.timeScale = 0;
        //replayButton.SetActive(true);
    }
    IEnumerator ExampleCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(1);
        Time.timeScale = 0;
        replayButton.SetActive(true);
    }
    public void OnReplayButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
