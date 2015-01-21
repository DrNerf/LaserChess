using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{
    public static AI Current;

    private Board CurrentBoard;
    private AIFigure[] AIUnits;

    public List<Drone> Drones = new List<Drone>();
    public List<Dreadnought> Dreadnoughts = new List<Dreadnought>();
    public List<CommandUnit> CommandUnits = new List<CommandUnit>();

    void Awake()
    {
        Current = this;
    }

	// Use this for initialization
    void Start()
    {
        CurrentBoard = Board.Current.GetComponent<Board>();
        AIUnits = FindObjectsOfType<AIFigure>() as AIFigure[];
        foreach (AIFigure figure in AIUnits)
        {
            if (figure.Type == FigureType.Drone)
                Drones.Add(figure.GetComponent<Drone>());
        }

        foreach (AIFigure figure in AIUnits)
        {
            if (figure.Type == FigureType.Dreadnought)
                Dreadnoughts.Add(figure.GetComponent<Dreadnought>());
        }

        foreach (AIFigure figure in AIUnits)
        {
            if (figure.Type == FigureType.CommandUnit)
                CommandUnits.Add(figure.GetComponent<CommandUnit>());
        }

        StartCoroutine(PlaySequence());
	}

    IEnumerator PlaySequence()
    {
        do
        {
            if (CurrentBoard.IsAITurn)
            {
                if (!HaveAllDronesMoved())
                {
                    foreach (Drone drone in Drones)
                    {
                        if (!drone.DidMove)
                        {
                            drone.MoveForward();
                            yield return new WaitForSeconds(3);
                            int temp = Random.Range(1, 3);
                            bool AttackDirection = (temp == 1) ? true : false;
                            drone.Attack(AttackDirection);
                            CurrentBoard.PassTurn();
                            break;
                        }
                    }
                }
                else
                {
                    if (!HaveAllDreadnoughtsMoved())
                    {
                        foreach (Dreadnought dreadnought in Dreadnoughts)
                        {
                            if (!dreadnought.DidMove)
                            {
                                dreadnought.MoveToRandom();
                                yield return new WaitForSeconds(3);
                                dreadnought.Attack();
                                CurrentBoard.PassTurn();
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (!HaveAllCommandUnitsMoved())
                        {
                            foreach (CommandUnit unit in CommandUnits)
                            {
                                if (!unit.DidMove)
                                {
                                    unit.MoveToRandom();
                                    yield return new WaitForSeconds(3);
                                    CurrentBoard.PassTurn();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ResetMovesLogic();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(5);
        } while (true);
    }

    /// <summary>
    /// Nothing to play, reset the logic algorithm to start all over.
    /// </summary>
    void ResetMovesLogic()
    {
        foreach (Drone drone in Drones)
        {
            drone.DidMove = false;
        }

        foreach (Dreadnought dreadnought in Dreadnoughts)
        {
            dreadnought.DidMove = false;
        }

        foreach (CommandUnit unit in CommandUnits)
        {
            unit.DidMove = false;
        }
    }

    /// <summary>
    /// Checks if all drones did their moves
    /// </summary>
    /// <returns>False if there is a drone that still did not moved.</returns>
    bool HaveAllDronesMoved()
    {
        foreach (Drone drone in Drones)
        {
            if (!drone.DidMove)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if all Dreadnoughts did their moves
    /// </summary>
    /// <returns>False if there is a Dreadnought that still did not moved.</returns>
    bool HaveAllDreadnoughtsMoved()
    {
        foreach (Dreadnought dreadnought in Dreadnoughts)
        {
            if (!dreadnought.DidMove)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Checks if all CommandUnits did their moves
    /// </summary>
    /// <returns>False if there is a CommandUnit that still did not moved.</returns>
    bool HaveAllCommandUnitsMoved()
    {
        foreach (CommandUnit unit in CommandUnits)
        {
            if (!unit.DidMove)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Destroys the object and remove it from the base
    /// </summary>
    /// <param name="drone">Drone to be destroyed</param>
    public void Destroy(Drone drone)
    {
        Drones.Remove(drone);
        Destroy(drone.gameObject);
    }

    /// <summary>
    /// Destroys the object and remove it from the base
    /// </summary>
    /// <param name="drone">Dreadnought to be destroyed</param>
    public void Destroy(Dreadnought dreadnaught)
    {
        Dreadnoughts.Remove(dreadnaught);
        Destroy(dreadnaught.gameObject);
    }

    /// <summary>
    /// Destroys the object and remove it from the base
    /// </summary>
    /// <param name="drone">Dreadnought to be destroyed</param>
    public void Destroy(CommandUnit unit)
    {
        CommandUnits.Remove(unit);
        Destroy(unit.gameObject);
        if (CommandUnits.Count < 1)
            Application.LoadLevel("Victory");
    }
}
