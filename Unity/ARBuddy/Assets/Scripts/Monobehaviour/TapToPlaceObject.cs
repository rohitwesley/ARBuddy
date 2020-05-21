using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Experimental.XR;


public class TapToPlaceObject : MonoBehaviour
{
    /// <summary>
    /// Basic AR Object Placement
    /// </summary>
    
    [SerializeField] private GameObject placmentIndicator;
    
    private ARSessionOrigin arOrigin;
    private Pose placmentPose;
    private bool placmentPoseIsValid = false;
    private ARRaycastManager raycastManager;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        arOrigin = FindObjectOfType<ARSessionOrigin>();  
    }

    void Update()
    {
        UpdatePlacmentPose();
        UpdatePlacmentIndicator();
    }

    private void UpdatePlacmentIndicator()
    {
        placmentIndicator.SetActive(placmentPoseIsValid);
        if(placmentPoseIsValid)
        {
            placmentIndicator.transform.SetPositionAndRotation(placmentPose.position, placmentPose.rotation);
        }
        else
        {

        }
    }

    private void UpdatePlacmentPose()
    {
        var ScreenCenter = Camera.current.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(ScreenCenter, hits, TrackableType.Planes);
        // If ray hit an object/plane
        placmentPoseIsValid = hits.Count>0;
        if(placmentPoseIsValid)
        {
            placmentPose = hits[0].pose;

            // change object rotation from scene origin to camera direction.
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placmentPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
    }
}
