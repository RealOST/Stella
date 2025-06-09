using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS.Systems {
    [UpdateAfter(typeof(TrackingSystem))]
    public partial struct TailSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (tail, speed, transform)
                     in SystemAPI.Query<
                         RefRO<Tail>,
                         RefRO<Speed>,
                         RefRW<LocalTransform>>()) {
                if (tail.ValueRO.Target == Entity.Null ||
                    !SystemAPI.HasComponent<LocalTransform>(tail.ValueRO.Target))
                    continue;

                var targetTransform = SystemAPI.GetComponent<LocalTransform>(tail.ValueRO.Target);
                var targetPos = targetTransform.Position;
                var currentPos = transform.ValueRO.Position;
                var followDistance = tail.ValueRO.Distance;

                // 计算目标方向和距离
                var toTarget = targetPos - currentPos;
                var distance = math.length(toTarget);

                // 如果距离大于需要保持的距离，就移动靠近
                if (distance > followDistance) {
                    var direction = math.normalize(toTarget);
                    var targetOffsetPos = targetPos - direction * followDistance;

                    // 计算每帧移动距离
                    var step = speed.ValueRO.Value * deltaTime;
                    var moveDir = targetOffsetPos - currentPos;
                    var moveDistance = math.length(moveDir);

                    if (moveDistance > 0) {
                        var moveStep = math.normalize(moveDir) * math.min(step, moveDistance);
                        transform.ValueRW.Position = currentPos + moveStep;
                    }
                }
            }
        }
    }
}