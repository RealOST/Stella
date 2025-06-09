using System.Collections.Generic;
using ECS.Authoring.Projectile;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ECS {
    public class EntityPool : MonoBehaviour {
        [SerializeField] private GameObject bulletPrefab; // 传统的 GameObject Prefab
        private Entity prefabEntity;

        private EntityManager entityManager;
        private Queue<Entity> entityPool = new();

        private void Start() {
            // 转换 GameObject Prefab 为 Entity
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            // prefabEntity = entityManager.Instantiate(bulletPrefab);
            entityManager.SetEnabled(prefabEntity, false); // 初始时禁用它
        }

        // 获取 Entity
        public Entity GetEntity(Vector3 position, Quaternion rotation) {
            Entity entity;
            if (entityPool.Count > 0)
                // 从池中获取一个 Entity
                entity = entityPool.Dequeue();
            else
                // 如果池中没有，实例化一个新的
                entity = entityManager.Instantiate(prefabEntity);

            // 设置实体的位置和旋转
            entityManager.SetComponentData(entity, new LocalTransform() { Position = position, Rotation = rotation });

            // 启用实体
            entityManager.SetEnabled(entity, true);

            return entity;
        }

        // 释放 Entity
        public void ReleaseEntity(Entity entity) {
            // 禁用并将其返回池中
            entityManager.SetEnabled(entity, false);
            entityPool.Enqueue(entity);
        }
    }
}