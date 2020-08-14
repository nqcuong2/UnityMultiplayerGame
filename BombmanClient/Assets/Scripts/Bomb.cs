using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int OwnerID { get; set; }
    public int Length { get; set; }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Boom!");
        GameManager.Instance.BombExploded(gameObject);
        Destroy(gameObject);
    }
}
