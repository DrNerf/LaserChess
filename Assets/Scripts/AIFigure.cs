using UnityEngine;
using System.Collections;

public class AIFigure : MonoBehaviour 
{
    public FigureType Type;
    /// <summary>
    /// If you want to decrease it, use TakeDamage() instead.
    /// </summary>
    public int HitPoints;
    public int AttackPower;
    public GameObject DeathEffect;

    private Vector3 MoveTarget = Vector3.zero;
    private Vector3 MoveTargetBackup = Vector3.zero;

    void Start()
    {
        MoveTarget = transform.position;
        MoveTargetBackup = transform.position;
    }
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, MoveTarget, Time.deltaTime * 5);
    }

    void OnTriggerEnter(Collider other)
    {
        Figure figure = other.GetComponent<Figure>();
        if (figure != null)
        {
            if (other.tag == "Figure" && figure.Type != FigureType.Jumpship)
            {
                Debug.Log("Collision with other figure, abording movement");
                MoveTarget = MoveTargetBackup;
            }
        }
        else
        {
            if (other.tag == "Figure")
            {
                Debug.Log("Collision with other figure, abording movement");
                MoveTarget = MoveTargetBackup;
            }
        }

        if (other.tag == "Finish")
        {
            Debug.Log("Hit the board wall, abording movement");
            MoveTarget = MoveTargetBackup;
        }

        if (Type == FigureType.Drone && other.tag == "8thRow")
            Application.LoadLevel("Lose");
    }

    /// <summary>
    /// Moves the figure to the specified position and sets backup.
    /// </summary>
    /// <param name="position">Position to move to</param>
    public void MoveTo(Vector3 position)
    {
        MoveTargetBackup = transform.position;
        MoveTarget = position;
    }

    /// <summary>
    /// Reduces the HitPoints by the amount of damage dealt.
    /// Will destroy it if the HitPoints get below 0.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        HitPoints -= damage;
        if (HitPoints < 1)
            Die();
    }

    void Die()
    {
        GameObject effect = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(effect, 5);
        if (Type == FigureType.Drone)
            AI.Current.Destroy(GetComponent<Drone>());
        if (Type == FigureType.Dreadnought)
            AI.Current.Destroy(GetComponent<Dreadnought>());
        if (Type == FigureType.CommandUnit)
            AI.Current.Destroy(GetComponent<CommandUnit>());
    }
}
