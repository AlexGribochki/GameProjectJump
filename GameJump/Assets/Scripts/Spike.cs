using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int damageAmount = 1; // количество урона

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hero hero = collision.GetComponent<Hero>();
        if (hero != null)
        {
            hero.GetDamage(damageAmount);
            Debug.Log("get collision");
        }
    }
}
