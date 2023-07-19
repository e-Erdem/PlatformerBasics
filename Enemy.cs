using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //public Animator animator;
    public int currentHealth = 100;
    public int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            IDie();
        }
    }

    void IDie()
    {
        Debug.Log("Idie Bye");
    }
}
