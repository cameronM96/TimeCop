using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseFunction : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    public bool pause;

    // Use this for initialization
    void Start()
    {
        //pauseMenu = GameObject.FindGameObjectWithTag("Pause");
        pauseMenu.SetActive(false);
        pause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
        }

        if (pause)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }
}
