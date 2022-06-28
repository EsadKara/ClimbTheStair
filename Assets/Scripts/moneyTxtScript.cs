using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class moneyTxtScript : MonoBehaviour
{
    Transform lookAt;
    TextMeshProUGUI textMesh;

    Camera cam;
    void Start()
    {
        cam = Camera.main;
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = System.String.Format("{0:0.0} $", gameManager.instance.income);
        lookAt = gameManager.instance.stairs[gameManager.instance.stairs.Count - 1].transform;
        transform.position = cam.WorldToScreenPoint(lookAt.position);
        DestroyObject();
    }

    void Update()
    {
        transform.position = cam.WorldToScreenPoint(lookAt.position);
        textMesh.color = Color.Lerp(textMesh.color, new Color(255, 255, 255, 0), 2f * Time.deltaTime);
        if (gameManager.instance.isPause)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    void DestroyObject()
    {
        Destroy(this.transform.parent.gameObject, 1f);
    }
}
