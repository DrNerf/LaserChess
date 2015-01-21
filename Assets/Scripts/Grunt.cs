using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Figure))]
public class Grunt : MonoBehaviour
{
    public Transform LeftDirection;
    public Transform RightDirection;
    public GameObject Laser;

    private Figure GruntFigure;
    private Text Tooltip;
    private bool IsLaserShot = false;
    private bool IsShotLeft = false;
    private bool IsSelected = false;

	// Use this for initialization
	void Start () 
    {
        GruntFigure = GetComponent<Figure>();
        Tooltip = GameObject.Find("Tooltip").GetComponent<Text>();
        Laser.GetComponent<Laser>().DamageToCarry = GruntFigure.AttackPower;
	}

    void Update()
    {
        if (IsLaserShot)
            if (IsShotLeft)
                Laser.transform.position = Vector3.Lerp(Laser.transform.position, LeftDirection.position, Time.deltaTime * 5);
            else
                Laser.transform.position = Vector3.Lerp(Laser.transform.position, RightDirection.position, Time.deltaTime * 5);
    }

    void OnGUI()
    {
        if (IsSelected)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 50, Screen.height - 70, 100, 50));
            GUILayout.BeginVertical();
            if (GUILayout.Button("Attack left"))
                AttackToLeft();
            if (GUILayout.Button("Attack right"))
                AttackToRight();
            GUILayout.BeginVertical();
            GUILayout.EndArea();
        }
    }

    void OnSelected()
    {
        IsSelected = GruntFigure.DidMoved;
        Tooltip.text = this.GetType().ToString() + " selected";
    }

    void OnDeSelected()
    {
        IsSelected = false;
        Tooltip.text = "";
    }

    /// <summary>
    /// Sends attack to the left diagonal
    /// </summary>
    void AttackToLeft()
    {
        Laser.SetActive(true);
        Laser.transform.position = transform.position;
        IsLaserShot = true;
        IsShotLeft = true;
        Invoke("DisableLaser", 1);
        GruntFigure.DeselectAll();
        Board.Current.PassTurn();
    }

    /// <summary>
    /// Sends attack to the right diagonal
    /// </summary>
    void AttackToRight()
    {
        Laser.SetActive(true);
        Laser.transform.position = transform.position;
        IsLaserShot = true;
        IsShotLeft = false;
        Invoke("DisableLaser", 1);
        GruntFigure.DeselectAll();
        Board.Current.PassTurn();
    }

    /// <summary>
    /// Disables the laser after use so we dont need to delete it
    /// </summary>
    void DisableLaser()
    {
        Laser.SetActive(false);
        IsLaserShot = false;
    }
}
