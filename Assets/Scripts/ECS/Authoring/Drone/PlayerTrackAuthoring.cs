using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class PlayerTrackAuthoring : MonoBehaviour {
        private class PlayerTrackAuthoringBaker : Baker<PlayerTrackAuthoring> {
            public override void Bake(PlayerTrackAuthoring authoring) {
                // 将当前GameObject转换成Entity
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // 创建一个组件，存储Entity引用
                AddComponent(entity, new PlayerTag());
            }
        }
    }
}