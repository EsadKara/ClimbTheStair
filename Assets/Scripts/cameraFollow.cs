using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class cameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        transform.position =  new Vector3(transform.position.x, player.transform.position.y + 1f, transform.position.z);
    }

    
    void LateUpdate()
    {
        if (!gameManager.instance.isComplete)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(
            transform.position.x, player.transform.position.y + 1.5f, transform.position.z), 0.01f);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(
           transform.position.x, player.transform.position.y + 2f, player.transform.position.z - 6.5f), 0.01f);
            transform.DORotate (new Vector3(20,0,0), 0.5f);
        }
        
    }
}
