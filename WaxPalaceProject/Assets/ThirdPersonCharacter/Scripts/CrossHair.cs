using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{

    [SerializeField] Texture2D image;
    [SerializeField] int size;
    [SerializeField] float maxAngle;
    [SerializeField] float minAngle;

    float lookHeight;

    public void LookHeight(float value)
    {
        lookHeight += value;
        if(lookHeight > maxAngle || lookHeight < minAngle)
        {
            lookHeight -= value;
        }
    }
    void onGUI()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        screenPosition = new Vector3(screenPosition.x, Screen.height - screenPosition.y, screenPosition.z);
        GUI.DrawTexture(new Rect(screenPosition.x, screenPosition.y - lookHeight, size, size), image);
    }


}
