using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
    public int DamageToCarry;
    public Owner LaserOwner;
    public GameObject HitParticles;
    public GameObject Parent;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
            gameObject.SetActive(false);

        if (other.tag == "Figure")
        {
            if (!other.gameObject.Equals(Parent))
            {
                AIFigure ai_figure = other.GetComponent<AIFigure>();
                if (ai_figure != null)
                {
                    if (LaserOwner != Owner.AI)
                    {
                        ai_figure.TakeDamage(DamageToCarry);
                        SpawnEffects();
                        gameObject.SetActive(false);
                    }
                    else
                        gameObject.SetActive(false);
                }
                else
                {
                    if (LaserOwner != Owner.Player)
                    {
                        Figure figure = other.GetComponent<Figure>();
                        figure.TakeDamage(DamageToCarry);
                        SpawnEffects();
                        gameObject.SetActive(false);
                    }
                    else
                        gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Shows hit effects and destroys them in 5 seconds.
    /// </summary>
    void SpawnEffects()
    {
        GameObject effect = Instantiate(HitParticles, transform.position, Quaternion.identity) as GameObject;
        Destroy(effect, 5);
    }
    
}

public enum Owner
{
    Player,
    AI
}