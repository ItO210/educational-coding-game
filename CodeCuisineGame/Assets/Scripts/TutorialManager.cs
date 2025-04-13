using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popUps;
    public int popUpIndex;
    [SerializeField] GameObject dropzone;
    [SerializeField] GameObject buildButton;
    [SerializeField] GameObject nivel1Button;


    IEnumerator waitSecond()
    {
        yield return new WaitForSeconds(0.1f);
        if (dropzone.transform.childCount == 0) {
            popUpIndex = 8;
        }
    }

    void Update() {
        for (int i = 0; i < popUps.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUps[i].SetActive(true);  // Set the current popup at index i to active
            }
            else
            {
                popUps[i].SetActive(false); // Set all other popups to inactive
            }
        }


        if (popUpIndex == 4)
        {
            try
            {
                if (dropzone.transform.childCount > 0)
                {
                    popUpIndex++;
                }
            }
            catch { }

        }
        else if (popUpIndex == 5)
        {
            try
            {
                if (dropzone.transform.GetChild(0).GetChild(0).GetChild(0))
                {
                    popUpIndex++;
                }
            }
            catch { }
        }
        else if (popUpIndex == 6)
        {
            try
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SCR_GetChildrenTut c = buildButton.GetComponent<SCR_GetChildrenTut>();
                    if (c.children.Count > 0) { popUpIndex++; }
                }
            }
            catch { }
        }
        else if (popUpIndex == 7)
        {
            try
            {
                if (dropzone.transform.childCount == 0)
                {
                    StartCoroutine(waitSecond());
                }
            }
            catch { }

        }
        else if (popUpIndex == 8)
        {
            buildButton.gameObject.SetActive(true);
            nivel1Button.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                popUpIndex++;
            }

        }
        else if (popUpIndex == 9) {
            PlayerPrefs.SetString("Tutorial", "Completado");
        }
    }
}
