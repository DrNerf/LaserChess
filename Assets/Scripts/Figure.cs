using UnityEngine;
using System.Collections;

public class Figure : MonoBehaviour
{
    public FigureType Type;
    /// <summary>
    /// If you want to decrease it, use TakeDamage() instead.
    /// </summary>
    public int HitPoints;
    public int AttackPower;
    public Transform MovableSpaces;
    public Transform AttackableSpaces;
    public bool DidMoved = false;
    public GameObject DeathEffect;

    private Light SelectedEffect;
    private Figure[] AllFigures;
    private Board CurrentGameBoard;
    private bool IsMoving = false;
    private Vector3 MoveTarget;
    private Vector3 MoveTargetBackup;

    void Start()
    {
        MoveTargetBackup = transform.position;
        SelectedEffect = GetComponent<Light>();
        AllFigures = FindObjectsOfType<Figure>() as Figure[];
        CurrentGameBoard = Board.Current.GetComponent<Board>();
    }

    void Update()
    {
        if (IsMoving)
            transform.position = Vector3.Lerp(transform.position, MoveTarget, Time.deltaTime * 5);

        if (Input.GetKeyDown(KeyCode.Escape))
            DeselectAll();
    }

    /// <summary>
    /// Moves the figure to the specified position
    /// </summary>
    /// <param name="position">The position to move to</param>
    public void MoveTo(Vector3 position)
    {
        DeselectAll();
        MoveTargetBackup = transform.position;
        CurrentGameBoard.IsLocked = true;
        if (Type == FigureType.Grunt)
        {
            position.y = 0.8f;
            position.x += 0.10f;
            position.z += 0.10f;
            MoveTarget = position;
            StartCoroutine(Move());
        }
        if (Type == FigureType.Jumpship)
        {
            position.y = 0.8f;
            position.x -= 0.05f;
            position.z += 0.15f;
            MoveTarget = position;
            StartCoroutine(Move());
        }
        if (Type == FigureType.Tank)
        {
            position.y = 0.33f;
            position.z += 0.15f;
            MoveTarget = position;
            StartCoroutine(Move());
        }
        DidMoved = true;
    }

    IEnumerator Move()
    {
        IsMoving = true;
        yield return new WaitForSeconds(3);
        IsMoving = false;
    }

    void OnSelected()
    {
        if (CurrentGameBoard.IsLocked)
        {
            if (CurrentGameBoard.SelectedFigure == this)
                SelectedSequence();
        }
        else
            SelectedSequence();
    }

    void SelectedSequence()
    {
        CurrentGameBoard.SelectedFigure = this;
        DeselectAll();
        SelectedEffect.enabled = true;
        Invoke("EnableAttackableSpaces", 0.5f);
        Invoke("EnableMovableSpaces", 0.5f);
    }

    private void OnDeSelected()
    {
        SelectedEffect.enabled = false;
        DisableAttackableSpaces();
        DisableMovableSpaces();
    }

    void OnMouseDown()
    {
        if(!CurrentGameBoard.IsAITurn)
            SendMessage("OnSelected");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Figure" && Type != FigureType.Jumpship)
            AbordMovement();
    }

    /// <summary>
    /// Abords the move of the figure
    /// </summary>
    void AbordMovement()
    {
        MoveTarget = MoveTargetBackup;
        DidMoved = false;
    }

    /// <summary>
    /// Deselects all figures on the board
    /// </summary>
    public void DeselectAll()
    {
        foreach (Figure figure in AllFigures)
        {
            if(figure != null)
                figure.SendMessage("OnDeSelected");
        }
    }

    void EnableMovableSpaces()
    {
        if (!DidMoved)
        {
            Vector3 pos = MovableSpaces.localPosition;
            pos.y = 0;
            MovableSpaces.localPosition = pos;
        }
    }

    void DisableMovableSpaces()
    {
        Vector3 pos = MovableSpaces.localPosition;
        pos.y = 5;
        MovableSpaces.localPosition = pos;
    }

    void EnableAttackableSpaces()
    {
        if (DidMoved)
        {
            Vector3 pos = AttackableSpaces.localPosition;
            pos.y = 0;
            AttackableSpaces.localPosition = pos;
        }
    }

    void DisableAttackableSpaces()
    {
        Vector3 pos = AttackableSpaces.localPosition;
        pos.y = 5;
        AttackableSpaces.localPosition = pos;
    }

    void ResetPaths()
    {
        Debug.Log("Reseting paths");
        //transform.localRotation = Quaternion.identity;
        AttackableSpaces.localPosition = new Vector3(0, 5, 0);
        MovableSpaces.localPosition = new Vector3(0, 5, 0);
    }

    /// <summary>
    /// Resets the figure cooldowns for the next round
    /// </summary>
    public void ResetForNextRound()
    {
        DidMoved = false;
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
        {
            GameObject effect = Instantiate(DeathEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(effect, 5);
            CurrentGameBoard.PlayerFigureDied();
            Destroy(gameObject);
        }
    }
}

public enum FigureType
{
    Grunt,
    Jumpship,
    Tank,
    Drone,
    Dreadnought,
    CommandUnit
}
