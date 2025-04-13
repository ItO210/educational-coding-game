using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 1
using TMPro; // Para los componentes TextMeshPro

public class LoadingAnimation : MonoBehaviour
{
    public Image circle; //2
    public float deltaTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        circle = GetComponent<Image>(); //3
    }

    // Update is called once per frame
    void Update()
    {
        //4
        deltaTime += Time.deltaTime; // Time.deltaTime es tiempo entre frames        
        circle.fillAmount = Mathf.PingPong(deltaTime, 1.0f);
    }
}
