
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RotateLootObject : UdonSharpBehaviour
{
    private void Update()
    {
        this.transform.Rotate(Vector3.forward, 90f * Time.deltaTime);
    }
}
