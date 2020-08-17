using UnityEngine;
using System.Collections.Generic;


public class NodeLinks : MonoBehaviour {
  public List<NodeLink> links;

  public void Update(){
    foreach(var link in links){
      Debug.DrawLine(link.a.transform.position, link.b.transform.position, Color.blue);
    }
  }

}