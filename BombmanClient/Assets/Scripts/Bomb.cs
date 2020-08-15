using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int ownerID;
    public int row;
    public int col;
    public int length;

    private void Awake()
    {
        length = 2;
    }

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Boom!");
        GameManager.Instance.BombExploded(this);
        Destroy(gameObject);
    }
}
