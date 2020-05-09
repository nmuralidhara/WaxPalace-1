using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public Text text;
    public float total_time = 1000;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void AddTime(){
        total_time += 10;
    }

    // Update is called once per frame
    void Update()
    {
        total_time -= Time.deltaTime;
        text.text = "Time Remaining: " + ((int)(total_time)).ToString();
        if (total_time <= 0)
        {
            //END THE GAME
            SceneManager.LoadScene("loose_screen");
        }
    }
}
