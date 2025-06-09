using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring.Projectile {
    // Runtime
    [InternalBufferCapacity(10)]
    internal struct ProjectilePrefab : IBufferElementData {
        public Entity EntityPrefab;
    }

    public class ProjectileStoreAuthor : MonoBehaviour {
        public ProjectileAuthoring[] projectiles;

        private class Baker : Baker<ProjectileStoreAuthor> {
            public override void Bake(ProjectileStoreAuthor authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                var dronePrefabs = AddBuffer<ProjectilePrefab>(entity);
                dronePrefabs.Capacity = authoring.projectiles.Length;

                for (var i = 0; i < authoring.projectiles.Length; i++)
                    dronePrefabs.Add(new ProjectilePrefab() {
                        EntityPrefab = GetEntity(authoring.projectiles[i],
                            TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace)
                    });
            }
        }
    }
}