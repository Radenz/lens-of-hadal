using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quest_giver : MonoBehaviour
{
  public Quest_menu quest;


  public GameObject quest_menu;
  public TextMeshProUGUI title;
  public TextMeshProUGUI description;
  public TextMeshProUGUI currency; 


public void questwindows(){
  quest_menu.SetActive(true);
  title.text=quest.title; 
}
  public void AcceptQuest()
  {
    quest_menu.SetActive(false);
    quest.isActive=true;
    player.quest = quest;
  }
}
