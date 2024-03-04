using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroButton : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
