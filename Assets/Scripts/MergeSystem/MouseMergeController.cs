using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MouseMergePointUpdated))]
public class MouseMergeController : MonoBehaviour
{
    [SerializeField] private LayerMask flyLayerMask;
    [SerializeField] private float YFly;
    private MouseMergePointUpdated mouseMergePointUpdated;

    private MergeObject attachObject;
    private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        mouseMergePointUpdated = GetComponent<MouseMergePointUpdated>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0)) OnMouseUp();
        if (Input.GetMouseButtonDown(0)) OnMouseDown();
        if (attachObject != null)
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 99, flyLayerMask))
            {
                attachObject.transform.DOKill();
                var recalculatePos = hit.point;
                recalculatePos.y = YFly;
                attachObject.transform.position = recalculatePos;
            }
        }
    }

    private void OnMouseUp()
    {
        mouseMergePointUpdated.UpdatePointVisible(null, false);
        mouseMergePointUpdated.UpdateVisible();
        if (attachObject == null)
        {
            return;
        }

        attachObject.SetFly(false);
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var mergePoint = hit.transform.GetComponent<MergePoint>();
            if (mergePoint)
            {
                mergePoint.AttachMergeObject(attachObject);
                attachObject = null;
            }
            else
            {
                attachObject.ReturnToAttachPoint();
                attachObject = null;
            }
        }
    }

    private void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            var mergePoint = hit.transform.GetComponent<MergePoint>();
            if (mergePoint && mergePoint.IsPointFree == false)
            {
                attachObject = mergePoint.CurrentMergeObject;
                attachObject.SetFly(true);
                mouseMergePointUpdated.UpdatePointVisible(attachObject, true);
            }
        }
    }
}