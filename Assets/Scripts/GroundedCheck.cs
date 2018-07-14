using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour {

    // floor detection
    private ContactFilter2D floorContactFilter = new ContactFilter2D();
    private Collider2D[] overlappingColliders;
    public bool isGrounded { get; private set; }
    public Collider2D objectCollider;
    private int maxNumberOfOverlappingColliders = 5;
    public float rayLenght;
    private RaycastHit2D raycastHit;
    public Vector2 groundindPoint { get; private set; }

	private void Awake()
	{
        floorContactFilter.layerMask = 1 << LayerMask.NameToLayer("Floor");
        overlappingColliders = new Collider2D[maxNumberOfOverlappingColliders];
	}

	private void Update()
	{
        CheckGround();
	}

	private void CheckGround()
    {
        // check using raycast first
        raycastHit = Physics2D.Raycast(transform.position, Vector2.down, rayLenght, 1 << LayerMask.NameToLayer("Floor"));
        if(raycastHit.collider != null)
        {
            isGrounded = true;
            groundindPoint = raycastHit.point;
            return;
        }


        // check using overlapping boxes
        overlappingColliders = new Collider2D[maxNumberOfOverlappingColliders];
        objectCollider.OverlapCollider(floorContactFilter, overlappingColliders);

        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if (overlappingColliders[i] == null)
            {
                break;
            }

            if (overlappingColliders[i].GetComponent<Floor>() != null)
            {
                isGrounded = true;
                groundindPoint = transform.position;
                return;
            }
        }

        isGrounded = false;
    }

	private void OnDrawGizmos()
	{
        Debug.DrawRay(transform.position, Vector3.down * rayLenght);
	}
}
