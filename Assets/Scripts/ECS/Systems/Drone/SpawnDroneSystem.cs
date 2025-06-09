using ECS.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems {
    [UpdateBefore(typeof(TransformSystemGroup))]
    public partial struct SpawnDroneSystem : ISystem {
        private Random m_Random;

        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<PlayerTag>();
            m_Random = new Random(1234);

            state.RequireForUpdate<DronePrefab>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var buffer = SystemAPI.GetSingletonBuffer<DronePrefab>();
            //
            // if (buffer.Length == 0 || m_SpawnCount >= 10)
            //     return;
            //
            // // 随机选择一个 prefab 实例化
            // var index = m_Random.NextInt(0, buffer.Length);
            // var drone = state.EntityManager.Instantiate(buffer[index].EntityPrefab);
            // var tail = SystemAPI.GetComponent<Tail>(drone);
            // tail.Value = index;
            // SystemAPI.SetComponent(drone, tail);
            //
            // // 设置初始位置（可选）
            // // var transform = SystemAPI.GetComponent<LocalTransform>(drone);
            // // transform.Position.x = m_SpawnCount * 2;
            // // SystemAPI.SetComponent(drone, transform);
            //
            // m_SpawnCount++;

            var ecb = new EntityCommandBuffer(Allocator.Temp);

            // 临时变量，用于记录上一个无人机实体
            var prev = Entity.Null;
            var player = SystemAPI.GetSingletonEntity<PlayerTag>();

            // 生成逻辑（示例生成5架跟随无人机）
            for (var i = 0; i < 4; i++) {
                var drone = ecb.Instantiate(buffer[0].EntityPrefab);

                ecb.SetComponent(drone,
                    i != 0 ? new Tail { Target = prev, Distance = 0.8f } : new Tail { Target = player });

                prev = drone;
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose(); // 别忘了释放 ECB！
            state.Enabled = false; // 只执行一次
        }
    }
}