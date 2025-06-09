using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems {
    [BurstCompile]
    public partial struct SurroundSystem : ISystem {
        public void OnUpdate(ref SystemState state) {
            var elapsedTime = (float)SystemAPI.Time.ElapsedTime;

            foreach (var (surround, localTransform) in SystemAPI.Query<
                         RefRO<Surround>, // 只读环绕参数
                         RefRW<LocalTransform>>() // 可读写位置
                    ) {
                var player = SystemAPI.GetSingletonEntity<PlayerTag>();
                
                // 读取父对象的位置
                var parentPos = SystemAPI.GetComponentRO<LocalTransform>(player).ValueRO.Position;

                // 计算当前角度
                var angle = elapsedTime * surround.ValueRO.Value;
                var offset = new float3(math.cos(angle), math.sin(angle), 0) * surround.ValueRO.Value;

                // 设置子机位置
                localTransform.ValueRW.Position = parentPos + offset;
            }
        }
    }
}