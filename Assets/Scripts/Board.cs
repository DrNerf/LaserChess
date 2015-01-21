using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour 
{
    public static Board Current;

    public Figure SelectedFigure;
    public bool IsLocked = false;
    public bool IsAITurn = false;
    public GameObject AITurnIcon;
    public GameObject PlayerTurnIcon;

    private int PlayerUnits;
    private Figure[] PlayerFigures;

	// Use this for initialization
	void Awake() 
    {
        Current = this;
	}

    void Start()
    {
        PlayerFigures = FindObjectsOfType<Figure>() as Figure[];
        PlayerUnits = PlayerFigures.Length;
    }

    /// <summary>
    /// Passes the turn to the opponent to play
    /// </summary>
    public void PassTurn()
    {
        IsAITurn = !IsAITurn;
        foreach (Figure figure in PlayerFigures)
        {
            if (figure != null)
                figure.ResetForNextRound();
        }
        AITurnIcon.SetActive(IsAITurn);
        PlayerTurnIcon.SetActive(!IsAITurn);
        IsLocked = false;
    }

    /// <summary>
    /// Notify the board that a player figure have died.
    /// </summary>
    public void PlayerFigureDied()
    {
        PlayerUnits--;
        if (PlayerUnits < 1)
            Application.LoadLevel("Lose");
    }
}
