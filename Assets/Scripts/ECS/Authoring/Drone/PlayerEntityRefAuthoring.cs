using Unity.Entities;
using UnityEngine;

namespace ECS.Authoring {
    public class PlayerEntityRefAuthoring : MonoBehaviour {
        [HideInInspector] public Entity PlayerEntity;

        public class Baker : Baker<PlayerEntityRefAuthoring> {
            public override void Bake(PlayerEntityRefAuthoring authoring) {
                // 将当前GameObject转换成Entity
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // 创建一个组件，存储Entity引用
                AddComponent(entity, new PlayerTag());
                AddComponentObject(entity, authoring); // 注意是Object组件！方便双向引用
            }
        }
    }
}