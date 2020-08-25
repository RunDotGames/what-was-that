using UnityEngine;
using System;
using System.Collections.Generic;

public class MeshDimmer : MonoBehaviour {

public MeshRenderer aRenderer;

private bool isDim;
private Coroutine dimmer;
private float dimLevel = 1;
private float dimSpeed;
private float dimTo;
private Color matColor;
private Material material;
private bool ignored;

public void Init(float dimSpeed, float dimTo, bool ignored){
  this.dimSpeed = dimSpeed;
  this.dimTo = dimTo;
  this.matColor = aRenderer.material.color;
  this.material = GameObject.Instantiate(aRenderer.material);
  aRenderer.material = this.material;
  this.ignored = ignored;
}

 public void Dim(){
   if(isDim || ignored){
     return;
   }
   if(dimmer != null){
     StopCoroutine(dimmer);
   }
   isDim =  true;
   dimmer = StartCoroutine(RunDimmer(1.0f, 0.4f));
 } 

 public void UnDim(){
   if(!isDim || ignored){
     return;
   }
   if(dimmer != null){
     StopCoroutine(dimmer);
   }
   isDim = false;
   dimmer = StartCoroutine(RunDimmer(0.4f, 1.0f));
 }

  

  private IEnumerator<YieldInstruction> RunDimmer(float fromLevel, float targetLevel){
   float elapsed = 0;
   while(true) {
     if(elapsed >= dimSpeed){
       aRenderer.material.color = new Color(matColor.r, matColor.g, matColor.b, targetLevel);
       break;
     }
     dimLevel = Mathf.Lerp(fromLevel, targetLevel, elapsed/dimSpeed);
     elapsed += Time.deltaTime;
     aRenderer.material.color = new Color(matColor.r, matColor.g, matColor.b, dimLevel);
     yield return new WaitForEndOfFrame();
   }
   dimmer = null;
 }

}