using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIFigure))]
public class CommandUnit : MonoBehaviour 
{
    public Transform LeftPath;
    public Transform RightPath;
    public bool DidMove = false;

    private AIFigure CommandUnitFigure;

    void Start()
    {
        CommandUnitFigure = GetComponent<AIFigure>();
    }

    /// <summary>
    /// Moves the figure 1 space to left
    /// </summary>
    public void MoveToLeft()
    {
        CommandUnitFigure.MoveTo(LeftPath.position);
        DidMove = true;
    }

    /// <summary>
    /// Moves the figure 1 space to right
    /// </summary>
    public void MoveToRight()
    {
        CommandUnitFigure.MoveTo(RightPath.position);
        DidMove = true;
    }

    /// <summary>
    /// The figure skips the turn deciding to do nothing
    /// </summary>
    public void Stay()
    {
        DidMove = true;
    }

    /// <summary>
    /// Moves the figure randomly in the 2 directions.
    /// Includes the chance to stay still
    /// </summary>
    public void MoveToRandom()
    {
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                MoveToLeft();
                break;
            case 1:
                MoveToRight();
                break;
            default:
                Stay();
                break;
        }
    }
}
