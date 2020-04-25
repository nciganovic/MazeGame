using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public float time;

    // Update is called once per frame
    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime * 2;

        if (time > 5) {
            SwitchScenes();
        }
    }

    void SwitchScenes() {
        SceneManager.LoadScene("MainMenu");
    }
}
