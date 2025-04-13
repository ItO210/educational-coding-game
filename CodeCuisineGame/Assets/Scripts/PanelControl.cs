using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelControl : MonoBehaviour
{
    [SerializeField]
    TMP_Text textMessage;

    public void SetMessage(string msg)
    {
        textMessage.text = msg;
    }

    public void DoOK()
    {
        this.gameObject.SetActive(false);
    }
}
