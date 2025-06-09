using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems {
    public partial class TrackingSystem : SystemBase {
        public static Entity PlayerEntity;
        private GameObject _player;

        protected override void OnStartRunning() {
            foreach (var (_, entity) in SystemAPI.Query<RefRO<PlayerTag>>().WithEntityAccess()
                         .WithAll<PlayerTag>())
                PlayerEntity = entity;
        }

        protected override void OnUpdate() {
            if (PlayerEntity == Entity.Null) {
                return;
            }
            if (_player == null) _player = GameObject.FindGameObjectWithTag("TailPosition");

            SystemAPI.SetComponent(PlayerEntity, new LocalTransform() {
                Position = _player.transform.position
            });
        }
    }
}