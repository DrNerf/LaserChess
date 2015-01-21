using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour 
{
    public GameObject MainPanel;
    public GameObject InfoPanel;

    public void LoadScene(int scene)
    {
        Application.LoadLevel(scene);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
