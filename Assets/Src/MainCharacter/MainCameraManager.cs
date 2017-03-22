using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraManager : MonoBehaviour
{

    public float dampTime = 0.15f;
    public Transform target;
    public float orthoSize = 0.5f;

    private Vector3 velocity = Vector3.zero;
    private Camera cameraComponent;

    private void Awake()
    {
        cameraComponent = this.GetComponent<Camera>();
        cameraComponent.orthographicSize = orthoSize; // this brings the camera closer to the target but for some reason wont save after changing it in the editor???? 
    }

    private void FixedUpdate()
    {
        if (target)
        {
            Vector3 point = cameraComponent.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cameraComponent.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }

    private void LateUpdate()
    {
        // get out of there if no target
        if (target == null)
        {
            return;
        }

        // get out if for some reason this script isnt attached to a camera 
        if(cameraComponent == null)
        {
            return;
        }
    }
}
