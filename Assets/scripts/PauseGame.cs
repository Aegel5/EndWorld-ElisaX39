using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseGame : MonoBehaviour
{
    public Transform canvas;

    FirstPersonController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
                player.enabled = false;
            }
            else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
                player.enabled = true;
            }
        }
    }
}
