using ECS.Systems;
using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class TailDroneAuthoring : MonoBehaviour {
        public PlayerTrackAuthoring target;
        public float distance = 0.8f;
        public float speed = 10f;

        private class Baker : Baker<TailDroneAuthoring> {
            public override void Bake(TailDroneAuthoring authoring) {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var targetEntity = GetEntity(authoring.target, TransformUsageFlags.Dynamic);

                AddComponent(entity, new Tail() { Target = targetEntity, Distance = authoring.distance });
                AddComponent(entity, new Speed() { Value = authoring.speed });
                AddComponent<DroneTag>(entity);
            }
        }
    }
}