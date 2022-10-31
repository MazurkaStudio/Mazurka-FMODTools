using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
/// <summary>
/// Use to set constant attenuation distance, no matter the camera distance ( in 2D game )
/// </summary>
public class FMOD_2DCameraConstantAttenuationDistance : MonoBehaviour
{
    [SerializeField] private float constantZDistance;

    public void SetConstantZDistance(float x) => constantZDistance = x;

    Vector3 workspace;

    private void Reset()
    {
        constantZDistance = transform.position.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        workspace = new Vector3(transform.position.x, transform.position.y, constantZDistance);
    }

    // Update is called once per frame
    void Update()
    {
        workspace.Set(transform.position.x, transform.position.y, constantZDistance);

        transform.position = workspace;
    }
}
