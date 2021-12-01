using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragToMoveController : MonoBehaviour
{
    public Transform draggable;
    public Transform debugSphere;
    public Transform leftDragger, rightDragger;
    public InputActionReference dragLeft;
    public InputActionReference dragRight;

    private Vector3 draggerStartLeft = Vector3.zero, draggerStartRight = Vector3.zero, draggableStart = Vector3.zero;
    private Quaternion dragableRotationStart;
    private Vector3 draggableScaleStart;
    private bool isDraggingLeft, isDraggingRight;
    

    private void Awake() {
        dragLeft.action.started += DragBeginLeft;
        dragRight.action.started += DragBeginRight;

        dragLeft.action.canceled += DragCompleteLeft;
        dragRight.action.canceled += DragCompleteRight;
    }

    private void OnDestroy() {
        dragLeft.action.started -= DragBeginLeft;
        dragRight.action.started -= DragBeginRight;

        dragLeft.action.canceled -= DragCompleteLeft;
        dragRight.action.canceled -= DragCompleteRight;
    }

    void Update()
    {
        if (isDraggingLeft && isDraggingRight)
        {
            // I think this can all be done with a single matrix multiplication, but I'm not smart enough for that right now
            Vector3 center = (leftDragger.position + rightDragger.position) / 2;
            // debugSphere.position = center;

            // Vector3 newDraggablePosition = draggableStart;
            // Quaternion newDraggableRotation = dragableRotationStart;

            // Scale
            float origDist = (draggerStartLeft - draggerStartRight).magnitude;
            float newDist = (leftDragger.position - rightDragger.position).magnitude;
            float scaleFactor = draggableScaleStart.x * newDist / origDist;
            // ScaleAround(draggable, center, new Vector3(scaleFactor, scaleFactor, scaleFactor));
            // draggable.position = center + (draggable.localPosition - center) * (scaleFactor / draggable.transform.localScale.x);
            draggable.localPosition = center + (draggable.localPosition - center) * (scaleFactor / draggable.transform.localScale.x);
            draggable.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

            // // Rotation
            // Quaternion deltaRotation = Quaternion.FromToRotation(draggerStartLeft - draggerStartRight, leftDragger.position - rightDragger.position);
            // // Vector3 localCenter = draggable.worldToLocalMatrix * center;
            // draggable.position = deltaRotation * (draggable.position - center) + center;
            // // newDraggablePosition = (deltaRotation * (center + newDraggablePosition));
            // draggable.rotation = deltaRotation * dragableRotationStart;



            // draggable.position = deltaRotation * (draggable.position - center) + center;
            // draggable.position = center + (draggable.localPosition - center) * (scaleFactor / draggable.transform.localScale.x);
            // draggable.position = deltaRotation * (  (draggable.position - center)) + center;

            // float angle = 0.0f;
            // Vector3 axis = Vector3.zero;
            // deltaRotation.ToAngleAxis(out angle, out axis);
            // draggable.rotation = dragableRotationStart;
            // draggable.RotateAround(center, axis, angle);
            // draggable.SetPositionAndRotation(newDraggablePosition, newDraggableRotation);
        }
        else if (isDraggingLeft)
        {
            Vector3 movementVector = leftDragger.position - draggerStartLeft;
            draggable.position = draggableStart + movementVector;
        }
        else if (isDraggingRight)
        {
            Vector3 movementVector = rightDragger.position - draggerStartRight;
            draggable.position = draggableStart + movementVector;
        }
        else
        {

        }
    }

    private void DragBeginLeft(InputAction.CallbackContext context)
    {
        isDraggingLeft = true;
        RecordInitialConditions();
    }

    private void DragBeginRight(InputAction.CallbackContext context)
    {
        isDraggingRight = true;
        RecordInitialConditions();
    }

    private void RecordInitialConditions()
    {
        draggerStartLeft = leftDragger.position;
        draggerStartRight = rightDragger.position;
        draggableStart = draggable.position;
        dragableRotationStart = draggable.rotation;
        draggableScaleStart = draggable.localScale;
    }

    private void DragCompleteLeft(InputAction.CallbackContext context)
    {
        isDraggingLeft = false;
        // todo remove
        isDraggingRight = false;
    }

    private void DragCompleteRight(InputAction.CallbackContext context)
    {
        isDraggingRight = false;
        // todo remove
        isDraggingLeft = false;
    }

    public void ScaleAround(Transform target, Vector3 pivot, Vector3 newScale)
    {
        Vector3 localTargetPosition = target.localPosition;
        Vector3 pivotToLocalTargetPosition = localTargetPosition - pivot;
        float relativeScaleFactor = newScale.x / target.transform.localScale.x;
        Vector3 finalPosition = pivot + pivotToLocalTargetPosition * relativeScaleFactor;
    
        target.localScale = newScale;
        target.localPosition = finalPosition;
        
        //

        // Vector3 finalPosition = pivot + (target.localPosition - pivot) * (newScale.x / target.transform.localScale.x);
    
        // target.localScale = newScale;
        // target.localPosition = finalPosition;
    }
     static void RotateAround (Transform transform, Vector3 pivotPoint, Quaternion rot)
    {
        transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;
    }
}
