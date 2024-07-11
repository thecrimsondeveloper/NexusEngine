using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AlignableObject : MonoBehaviour
{
    [SerializeField] Transform anchorOne;
    [SerializeField] Transform anchorTwo;
    [SerializeField] Pose targetPose;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetData();
        transform.position = targetPose.position;
        transform.rotation = targetPose.rotation;
    }
    void SetData(){

        targetPose.position= (anchorOne.position + anchorTwo.position)/2;
        Vector3 dirBetweenAnchors = anchorOne.position - anchorTwo.position;
        targetPose.rotation = Quaternion.LookRotation(dirBetweenAnchors);
    }
}
