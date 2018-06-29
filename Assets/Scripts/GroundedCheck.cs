using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedCheck : MonoBehaviour {

    // floor detection
    private ContactFilter2D floorContactFilter;
    private Collider2D[] overlappingColliders;
    public bool isGrounded { get; private set; }
    public Collider2D objectCollider;
    private int maxNumberOfOverlappingColliders;
    public float rayLenght;
    private RaycastHit2D raycastHit;

	private void Awake()
	{
        floorContactFilter = new ContactFilter2D();
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
            return;
        }


        // check using overlapping boxes
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
