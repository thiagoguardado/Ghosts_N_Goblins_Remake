using UnityEngine;

public class FloorChecker : MonoBehaviour
{

    [Header("FloorCheck")]
    public Transform floorRayCheckStart;
    public float floorRayLenght;
    public bool hasFloorAhead { get; private set; }

    private void Update()
    {
        CheckFloorAhead();
    }

    private void CheckFloorAhead()
    {
        hasFloorAhead = Physics2D.Raycast(floorRayCheckStart.position, Vector2.down, floorRayLenght, 1 << LayerMask.NameToLayer("Floor"));
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(floorRayCheckStart.position, Vector3.down * floorRayLenght);
    }
}
