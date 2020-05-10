using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void onClick()
    {
        GameObject t = GameObject.Find("gen_text");
        t.GetComponent<UnityEngine.UI.Text>().enabled = true;

        SceneManager.LoadScene("SampleScene");
    }
}
