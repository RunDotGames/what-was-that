using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class InstrController : MonoBehaviour {

  public GameObject[] instructions;
  public int currentIndex;

  public void Start(){
    foreach (var inst in instructions){
        inst.SetActive(false);
    }
    instructions[currentIndex].SetActive(true);
  }
 public void Update(){
   if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)){
     SceneManager.LoadScene(1);
     return;
   }
   if(Input.GetKeyUp(KeyCode.Escape)){
     Application.Quit();
   }
   if(Input.GetKeyUp(KeyCode.Space)){
     instructions[currentIndex].SetActive(false);
     currentIndex ++;
     if(currentIndex >= instructions.Length){
       SceneManager.LoadScene(1);
       return;
     }
     instructions[currentIndex].SetActive(true);
   }
 } 
}