using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Figure))]
public class Jumpship : MonoBehaviour
{
    public Transform[] Directions;
    public GameObject[] Lasers;

    private Figure JumpshipFigure;
    private Text Tooltip;
    private bool IsLaserShot = false;
    private bool IsSelected = false;

	// Use this for initialization
	void Start () 
    {
        JumpshipFigure = GetComponent<Figure>();
        Tooltip = GameObject.Find("Tooltip").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsLaserShot)
        {
            for (int i = 0; i < 4; i++)
            {
                Lasers[i].transform.position = Vector3.Lerp(Lasers[i].transform.position, Directions[i].position, Time.deltaTime * 20);
            }
        }
	}

    void OnGUI()
    {
        if (IsSelected && JumpshipFigure.DidMoved)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 50, Screen.height - 70, 100, 50));
            GUILayout.BeginVertical();
            if (GUILayout.Button("Attack", GUILayout.Width(90)))
                Attack();
            GUILayout.BeginVertical();
            GUILayout.EndArea();
        }
    }

    void OnSelected()
    {
        IsSelected = true;
        Tooltip.text = this.GetType().ToString() + " selected";
    }

    void OnDeSelected()
    {
        IsSelected = false;
        Tooltip.text = "";
    }

    void Attack()
    {
        ResetAllLasers();
        SetActiveToAllLasers(true);
        IsLaserShot = true;
        JumpshipFigure.DeselectAll();
        Invoke("DisableLasers", 0.35f);
        Board.Current.PassTurn();
    }

    void SetActiveToAllLasers(bool status)
    {
        foreach (GameObject laser in Lasers)
        {
            laser.SetActive(status);
        }
    }

    void ResetAllLasers()
    {
        foreach (GameObject laser in Lasers)
        {
            laser.transform.position = transform.position;
        }
    }

    void DisableLasers()
    {
        IsLaserShot = false;
        SetActiveToAllLasers(false);
    }
}
