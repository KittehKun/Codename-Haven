
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

//The purpose of this script is to disable all Enemy Spawn mesh renderers on runtime
public class DisableChildrenMeshRenderer : UdonSharpBehaviour
{
    void Start()
    {
        foreach(MeshRenderer meshRenderer in this.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }
}
