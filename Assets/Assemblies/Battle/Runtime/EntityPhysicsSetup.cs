using UnityEngine;

namespace ArmyClash.Battle.Physics
{
    [DisallowMultipleComponent]
    public sealed class EntityPhysicsSetup : MonoBehaviour
    {
        private void Reset()
        {
            SetupPhysicsComponents();
        }

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                SetupPhysicsComponents();
            }
        }

        [ContextMenu("Setup Physics Components")]
        private void SetupPhysicsComponents()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }

            rb.useGravity = false;
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            MeshCollider collider = GetComponent<MeshCollider>();
            if (collider == null)
            {
                collider = gameObject.AddComponent<MeshCollider>();
            }

            Mesh mesh = null;
            MeshFilter meshFilter = GetComponentInChildren<MeshFilter>();
            if (meshFilter != null)
            {
                mesh = meshFilter.sharedMesh;
            }
            else
            {
                SkinnedMeshRenderer skinned = GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinned != null)
                {
                    mesh = skinned.sharedMesh;
                }
            }

            if (mesh != null)
            {
                collider.sharedMesh = mesh;
            }

            collider.convex = true;
        }
    }
}
