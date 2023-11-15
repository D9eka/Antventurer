using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSprite : MonoBehaviour
{
    public float displayTimer = 5f;
    public float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > displayTimer){
            Destroy(gameObject);
        }
    }
}
