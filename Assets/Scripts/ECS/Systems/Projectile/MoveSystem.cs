using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS.Systems.Projectile {
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct MoveSystem : ISystem {
        public void OnCreate(ref SystemState state) {
            state.GetEntityQuery(typeof(Player));
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (xf, dir, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Direction>, RefRO<Speed>>()
                         .WithAll<ProjectileTag>()) {
                var direction = math.normalizesafe(dir.ValueRO.Value);
                var displacement = new float3(direction.x, direction.y, 0f) * speed.ValueRO.Value * deltaTime;

                xf.ValueRW.Position += displacement;

                // 朝向调整：让子弹“脸”朝向移动方向（绕Z轴旋转）
                var angle = math.atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                xf.ValueRW.Rotation = quaternion.Euler(math.radians(angle));
            }
        }
    }
}