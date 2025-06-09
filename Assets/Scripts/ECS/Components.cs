using Unity.Entities;
using Unity.Mathematics;

// 子机的标记组件
namespace ECS {
    #region Drone

    public struct PlayerTag : IComponentData {
    }

    public struct DroneTag : IComponentData {
    }

    public struct Tail : IComponentData {
        public Entity Target;
        public float Distance;
    }

    public struct Surround : IComponentData {
        public float Value; // 环绕半径
    }

    public struct Speed : IComponentData {
        public float Value;
    }

    #endregion

    #region Combat

    public struct ProjectileTag : IComponentData {
    }

    public struct Direction : IComponentData {
        public float2 Value;
    }

    public struct Damage : IComponentData {
        public float Value;
    }

    #endregion
}