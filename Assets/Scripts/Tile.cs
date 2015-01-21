using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour 
{
    public bool IsOccupied = false;
    public bool IsHighlighted = false;

    private Board CurrentGameBoard;

    void Start()
    {
        CurrentGameBoard = Board.Current.GetComponent<Board>();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Path")
            SendMessage("OnMoveableDisable");
        if (other.tag == "Figure")
            SendMessage("OnLeaved");
        if (other.tag == "AttackPath")
            SendMessage("OnAttackableDisable");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Path")
            SendMessage("OnMoveable");
        if (other.tag == "Figure")
            SendMessage("OnOccupied");
        if (other.tag == "AttackPath")
            SendMessage("OnAttackable");
    }

    void OnAttackable()
    {
        renderer.material.color = Color.magenta;
    }

    void OnAttackableDisable()
    {
        renderer.material.color = Color.white;
    }

    void OnOccupied()
    {
        IsOccupied = true;
        renderer.material.color = Color.red;
    }

    void OnLeaved()
    {
        IsOccupied = false;
        renderer.material.color = Color.white;
    }

    void OnMoveable()
    {
        IsHighlighted = true;
        renderer.material.color = Color.cyan;
    }

    void OnMoveableDisable()
    {
        IsHighlighted = false;
        renderer.material.color = Color.white;
    }

    void OnMouseDown()
    {
        if (!IsOccupied && IsHighlighted)
            CurrentGameBoard.SelectedFigure.MoveTo(transform.position);
    }
}
