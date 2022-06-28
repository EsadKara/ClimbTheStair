using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerControl : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform levelUpPos;
    Animator playerAnim;
    Quaternion newRot;
    bool isMove;
    void Start()
    {
        playerAnim = player.GetComponent<Animator>();
        isMove = true;
    }

  
    void Update()
    {
        if(!gameManager.instance.isComplete)
             playerAnim.SetBool("isClimb", gameManager.instance.isClimb);
        else
        {
            playerAnim.SetBool("isClimb", false);
            LevelUp();
        }
           
        if (gameManager.instance.stairs.Count > 1 && !gameManager.instance.isComplete)
        {
            transform.DOMove(new Vector3(
            gameManager.instance.stairs[gameManager.instance.stairs.Count - 2].transform.position.x,
            gameManager.instance.stairs[gameManager.instance.stairs.Count - 2].transform.position.y + 0.383001f,
            gameManager.instance.stairs[gameManager.instance.stairs.Count - 2].transform.position.z), 0.2f);
            transform.rotation = gameManager.instance.stairs[gameManager.instance.stairs.Count - 2].transform.rotation;
        }
        
    }

    void LevelUp()
    {
        if (isMove)
        {
            transform.DOMove(new Vector3(
            levelUpPos.position.x + 0.3f, levelUpPos.position.y + 1f, levelUpPos.position.z - 1.5f), 0.4f).OnComplete(() =>
             transform.DOMove(levelUpPos.position, 0.4f));
            isMove = false;
        }
        
    }
}
