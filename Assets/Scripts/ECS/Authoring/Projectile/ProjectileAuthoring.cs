using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring.Projectile {
    public class ProjectileAuthoring : MonoBehaviour {
        public float moveSpeed = 10f;
        public Vector2 moveDirection = Vector2.right;
    }

    public class ProjectileBaker : Baker<ProjectileAuthoring> {
        public override void Bake(ProjectileAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ProjectileTag());
            AddComponent(entity, new Speed { Value = authoring.moveSpeed });
            AddComponent(entity, new Direction { Value = authoring.moveDirection });
        }
    }
}