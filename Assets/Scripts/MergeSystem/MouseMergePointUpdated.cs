using UnityEngine;

public class MouseMergePointUpdated : MonoBehaviour
{
    private bool updatePointVisible;

    private MergePoint lastPoint;
    private MergeObject currentObjectMerge;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (updatePointVisible)
        {
            SetOutline();
        }
    }

    public void UpdatePointVisible(MergeObject mergeObject, bool isVisible)
    {
        updatePointVisible = isVisible;
        currentObjectMerge = mergeObject;
    }
    public void UpdateVisible()
    {
        if (lastPoint != null)
        {
            lastPoint.SetInduced(false);
            lastPoint.SetOutline(null);
            lastPoint = null;
        }
    }
    private void SetOutline()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var mergePoint = hit.transform.GetComponent<MergePoint>();
            if (mergePoint)
            {
                if (mergePoint == lastPoint) return;
                if (lastPoint != null)
                {
                    lastPoint.SetInduced(false);
                    lastPoint.SetOutline(null);
                }

                lastPoint = mergePoint;
                lastPoint.SetInduced(true);
                lastPoint.SetOutline(currentObjectMerge);
            }
            else
            {
                if (lastPoint != null)
                {
                    lastPoint.SetInduced(false);
                    lastPoint.SetOutline(null);
                    lastPoint = null;
                }
            }
        }
    }
}