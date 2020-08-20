using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeLinks))]
public class NodeLinksEditor : Editor {
    
    public void OnSceneGUI() {
        var nodeLinks = (NodeLinks)target;
        Handles.color = Color.blue;
        foreach(var link in nodeLinks.links) {
            if(link.a == null || link.b == null) {
                continue;
            }
            Handles.DrawLine(link.a.transform.position, link.b.transform.position);
        }
    }
}