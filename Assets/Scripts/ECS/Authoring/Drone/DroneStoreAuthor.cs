using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    // Runtime
    [InternalBufferCapacity(4)]
    internal struct DronePrefab : IBufferElementData {
        public Entity EntityPrefab;
    }

    public class DroneStoreAuthor : MonoBehaviour {
        public TailDroneAuthoring[] drones;

        private class Baker : Baker<DroneStoreAuthor> {
            public override void Bake(DroneStoreAuthor authoring) {
                var entity = GetEntity(TransformUsageFlags.None);
                var dronePrefabs = AddBuffer<DronePrefab>(entity);
                dronePrefabs.Capacity = authoring.drones.Length;

                for (var i = 0; i < authoring.drones.Length; i++)
                    dronePrefabs.Add(new DronePrefab() {
                        EntityPrefab = GetEntity(authoring.drones[i],
                            TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace)
                    });
            }
        }
    }
}