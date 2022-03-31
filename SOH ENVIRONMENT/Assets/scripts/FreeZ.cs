using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeZ : MonoBehaviour
{
    void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }
}
