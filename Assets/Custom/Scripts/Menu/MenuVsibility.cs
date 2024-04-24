using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuVisibility : MonoBehaviour
{
    public string nameMenu;
    [HideInInspector] public bool isOpen;

    public void Visible()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }
    public void NoVisible()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
