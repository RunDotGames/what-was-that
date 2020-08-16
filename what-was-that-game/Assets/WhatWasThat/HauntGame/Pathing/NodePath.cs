using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class NodeLink {
  public NodeItem a;
  public NodeItem b;
}

class NodeTraversal {
  public float distance;
  public List<string> guids;
  
  public NodeTraversal Clone(){
    var clone = new NodeTraversal();
    clone.distance = distance;
    clone.guids = guids.GetRange(0, guids.Count);
    return clone;
  }
}
public class NodeState {
  public NodeItem item;
  public Dictionary<string, NodeState> to = new Dictionary<string, NodeState>();
}

public class NodePath: MonoBehaviour {

  public List<NodeLink> links;

  private Dictionary<string, NodeState> nodes;

  private NodeState RequireState(NodeItem item){
    if (!nodes.ContainsKey(item.guid)) {
      var state = new NodeState();
      state.item = item;
      nodes[item.guid] = state;
    }
    return nodes[item.guid];

  }

  public void Start(){
    nodes = new Dictionary<string, NodeState>();
    foreach(var link in links) {
      var aState = RequireState(link.a);
      var bState = RequireState(link.b);
      bState.to[link.a.guid] = aState;
      aState.to[link.b.guid] = bState;
    }
  }

  public void Update(){
    links.ForEach((link) => {
      Debug.DrawLine(link.a.transform.position, link.b.transform.position, Color.blue);
    });
  }

  private NodeItem GetNearestNode(Vector3 position) {
    NodeItem nearest = null;
    float minDistance = float.MaxValue;
    foreach(var value in nodes.Values) {
      var myDistance = (value.item.transform.position - position).magnitude;
      if(myDistance < minDistance){
        minDistance = myDistance;
        nearest = value.item;
      }
    }
    return nearest;
  }

  private NodeTraversal GetTraversal(NodeTraversal prior, NodeItem next, NodeItem target) {
    NodeTraversal traversal = prior.Clone();
    
    if(traversal.guids.Count > 0){
      var last = nodes[prior.guids.Last()].item;
      traversal.distance = prior.distance + (last.transform.position - next.transform.position).magnitude;
    }
    traversal.guids.Add(next.guid);
    if(target.guid == next.guid) {
      return traversal;
    }

    var state = nodes[next.guid];
    NodeTraversal shortestTraversal = null;
    foreach (var to in state.to.Values) {
      if(traversal.guids.Contains(to.item.guid)){
        continue;
      }

      var nextTraversal = GetTraversal(traversal, to.item, target);
      if(nextTraversal == null) {
        continue;
      }

      if(shortestTraversal == null || nextTraversal.distance < shortestTraversal.distance) {
        shortestTraversal = nextTraversal;
      }
    }
    return shortestTraversal;
  }

  public List<NodeItem> GetRoute(Vector3 from, Vector3 to) {
    var fromNode = GetNearestNode(from);
    var toNode = GetNearestNode(to);
    if(fromNode.guid == toNode.guid) {
      return new List<NodeItem>(){fromNode};
    }

    var baseTraversal = new NodeTraversal(){distance = 0.0f, guids = new List<string>() {}};
    var pathTraversal = GetTraversal(baseTraversal, fromNode, toNode);
    if(pathTraversal == null) {
      return null;
    }
    return pathTraversal.guids.Select((guid) => nodes[guid].item).ToList();
    
  }
}