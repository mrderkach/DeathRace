using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage = 1;
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<CarGeneral>())
        {
            other.gameObject.GetComponent<CarGeneral>().CurrentHealth -= damage;
            other.gameObject.GetComponent<CarGeneral>()._manager.healthUI.GetComponent<HealthManager>().UpdateHealth(
                other.gameObject.GetComponent<CarGeneral>().CurrentHealth, other.gameObject);
        }
        Destroy(gameObject);
    }
}
