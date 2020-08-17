using UnityEngine;
using System.Collections.Generic;

public class InvestigatorDemo: MonoBehaviour {

  public InvestigatorController investigator;
  public Transform to;
  public List<NodeLinks> linkSets;

  public void Update(){
    if(Input.GetKeyDown(KeyCode.Space)){
      foreach(var linkSet in linkSets) {
        NodePathController.AddLinks(linkSet.links);
      }
      investigator.GoTo(to.position);
    }
  }

}