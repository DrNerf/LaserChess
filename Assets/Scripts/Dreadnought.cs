using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIFigure))]
public class Dreadnought : MonoBehaviour 
{
    public Transform[] Paths = new Transform[8];
    public GameObject[] Lasers = new GameObject[8];
    public Transform LasersParent;
    public GameObject LaserPrefab;
    public bool DidMove = false;

    private Vector3 MoveTarget;
    private bool IsLaserShot = false;
    private AIFigure DreadnoughtFigure;

	// Use this for initialization
	void Start () 
    {
        DreadnoughtFigure = GetComponent<AIFigure>();
        //MoveTarget = transform.position;

        for (int i = 0; i < Lasers.Length; i++)
        {
            Lasers[i] = Instantiate(LaserPrefab, transform.position, Quaternion.identity) as GameObject;
            Lasers[i].transform.parent = LasersParent;
            Laser laser = Lasers[i].GetComponent<Laser>();
            laser.Parent = gameObject;
            laser.DamageToCarry = DreadnoughtFigure.AttackPower;
            laser.LaserOwner = Owner.AI;
            Lasers[i].SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () 
    {
        //transform.position = Vector3.Lerp(transform.position, MoveTarget, Time.deltaTime * 5);

        if (IsLaserShot)
        {
            Lasers[0].transform.Translate(Vector3.forward * (Time.deltaTime * 20));
            Lasers[1].transform.Translate((Vector3.forward + Vector3.right) * (Time.deltaTime * 20));
            Lasers[2].transform.Translate(Vector3.right * (Time.deltaTime * 20));
            Lasers[3].transform.Translate((Vector3.right + Vector3.back) * (Time.deltaTime * 20));
            Lasers[4].transform.Translate(Vector3.back * (Time.deltaTime * 20));
            Lasers[5].transform.Translate((Vector3.back + Vector3.left) * (Time.deltaTime * 20));
            Lasers[6].transform.Translate(Vector3.left * (Time.deltaTime * 20));
            Lasers[7].transform.Translate((Vector3.left + Vector3.forward) * (Time.deltaTime * 20));
        }
	}

    /// <summary>
    /// Moves the figure to the specified position
    /// </summary>
    /// <param name="position">The index of the position 0 - 7</param>
    public void MoveTo(int position)
    {
        Vector3 pos = Paths[position].position;
        pos.z -= 0.1f;
        pos.x += 0.32f;
        //MoveTarget = pos;
        DreadnoughtFigure.MoveTo(pos);
        DidMove = true;
    }

    /// <summary>
    /// Moves the figure to random position
    /// </summary>
    public void MoveToRandom()
    {
        int rand = Random.Range(0, 8);
        MoveTo(rand);
    }

    /// <summary>
    /// Attacks all adjacent figures
    /// </summary>
    public void Attack()
    {
        StartCoroutine(AttackSequence());
    }

    private IEnumerator AttackSequence()
    {
        foreach (GameObject laser in Lasers)
        {
            laser.SetActive(true);
            Vector3 reset_position = transform.position;
            reset_position.x -= 0.3f;
            reset_position.z += 0.2f;
            reset_position.y = 0.5f;
            laser.transform.position = reset_position;
            IsLaserShot = true;
        }
        yield return new WaitForSeconds(0.05f);
        IsLaserShot = false;
        foreach (GameObject laser in Lasers)
        {
            laser.SetActive(false);
        }
    }
}
