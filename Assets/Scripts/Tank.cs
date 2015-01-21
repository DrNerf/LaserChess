using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Figure))]
public class Tank : MonoBehaviour 
{
    public GameObject Laser;

    private Figure TankFigure;
    private Text Tooltip;
    private bool IsLaserShot = false;
    private bool IsSelected = false;
    private AttackDirection LaserDirection;

	// Use this for initialization
	void Start () 
    {
        TankFigure = GetComponent<Figure>();
        Tooltip = GameObject.Find("Tooltip").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsLaserShot)
        {
            if (LaserDirection == AttackDirection.Forward)
                Laser.transform.Translate(Vector3.forward * (Time.deltaTime * 20));
            if (LaserDirection == AttackDirection.Right)
                Laser.transform.Translate(Vector3.right * (Time.deltaTime * 20));
            if (LaserDirection == AttackDirection.Backwards)
                Laser.transform.Translate(Vector3.back * (Time.deltaTime * 20));
            if (LaserDirection == AttackDirection.Left)
                Laser.transform.Translate(Vector3.left * (Time.deltaTime * 20));
        }
	}

    void OnGUI()
    {
        if (IsSelected && TankFigure.DidMoved)
        {
            GUILayout.BeginArea(new Rect(Screen.width / 2 - 80, Screen.height - 125, 160, 100));
            GUILayout.BeginVertical();
            if (GUILayout.Button("Attack forward", GUILayout.Width(120)))
                Attack(AttackDirection.Forward);
            if (GUILayout.Button("Attack left", GUILayout.Width(120)))
                Attack(AttackDirection.Left);
            if (GUILayout.Button("Attack right", GUILayout.Width(120)))
                Attack(AttackDirection.Right);
            if (GUILayout.Button("Attack backwards", GUILayout.Width(120)))
                Attack(AttackDirection.Backwards);
            GUILayout.BeginVertical();
            GUILayout.EndArea();
        }
    }

    void Attack(AttackDirection direction)
    {
        LaserDirection = direction;
        StartCoroutine(AttackSequence());
    }

    IEnumerator AttackSequence()
    {
        Laser.transform.position = transform.position;
        Laser.SetActive(true);
        IsLaserShot = true;
        TankFigure.DeselectAll();
        yield return new WaitForSeconds(1);
        IsLaserShot = false;
        Laser.SetActive(false);
        Board.Current.PassTurn();
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

    private enum AttackDirection
    {
        Left,
        Forward,
        Right,
        Backwards
    }
}
