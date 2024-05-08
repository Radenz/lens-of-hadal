using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour
{
    public void play_button(){
        SceneManager.LoadScene("save");
    }

    public void Setting(){
        
    }

    public void Quit(){
       Application.Quit();
    }
}
