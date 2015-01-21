using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AIFigure))]
public class Drone : MonoBehaviour 
{
    public Transform Path;
    public bool DidMove = false;
    public GameObject Laser;

    private AIFigure DroneFigure;
    private bool IsLaserShot = false;
    private Vector3 ShootDirection;

    void Start()
    {
        DroneFigure = GetComponent<AIFigure>();
    }

    void Update()
    {
        if (IsLaserShot)
            Laser.transform.Translate((Vector3.forward + ShootDirection) * (Time.deltaTime * 20));
    }

    /// <summary>
    /// Moves the Drone 1 position forward
    /// </summary>
    public void MoveForward()
    {
        DroneFigure.MoveTo(Path.position);
        DidMove = true;
    }

    /// <summary>
    /// Attacks in the specified direction
    /// </summary>
    /// <param name="IsShootingLeft">Set true if shooting to left and false if to right.</param>
    public void Attack(bool IsShootingLeft)
    {
        Laser.SetActive(true);
        if (IsShootingLeft)
            ShootDirection = Vector3.left;
        else
            ShootDirection = Vector3.right;
        Laser.transform.position = transform.position;
        IsLaserShot = true;
        Invoke("StopLaser", 1);
    }

    /// <summary>
    /// Disables the laser that has been shot
    /// </summary>
    void StopLaser()
    {
        IsLaserShot = false;
        Laser.SetActive(false);
    }
}
