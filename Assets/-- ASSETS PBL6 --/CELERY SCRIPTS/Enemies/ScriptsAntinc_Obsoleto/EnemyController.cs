using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Tipus d'enemic
    [SerializeField] private FoodType FoodType;
    [SerializeField] private int health = 1;
    [SerializeField] private float hitDuration;
    public void GetDamage(GameObject damager)
    {
        if (health > 1) //Damage
        {
            health--;
            StartCoroutine(HitEffect());
        }
        else //Mort
        {
            if(damager.CompareTag("Player")) damager.GetComponent<PanController>().ChangeFoodType(FoodType);
            Destroy(gameObject);
        }
        //Damage Animation [FALTA]
        //Sistema de Damage [FALTA]

        //Quan mor, canvia FoodType de Pan
        
    }

    IEnumerator HitEffect()
    {
        //Efectes quan és atacat
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 1.2f, transform.localScale.z);
        yield return new WaitForSeconds(hitDuration);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 1.2f, transform.localScale.z);
    }
}
