using UnityEngine;


public class InvestigatorDemo: MonoBehaviour {

  public InvestigatorController investigator;
  public Transform to;

  public void Update(){
    if(Input.GetKeyDown(KeyCode.Space)){
      investigator.GoTo(to.position);
    }
  }

}