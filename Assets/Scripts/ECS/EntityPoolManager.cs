using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ECS {
    public class EntityPoolManager : MonoBehaviour {
        [SerializeField] private EntityPool pool;
        [SerializeField] private float moveSpeed = 10f;

        public void SpawnBullet(Vector3 position, Quaternion rotation) {
            var projectileEntity = pool.GetEntity(position, rotation);

            // 启动子弹运动系统（ECS 系统）
            var moveSpeed = new Speed { Value = this.moveSpeed };
            var moveDirection = new LocalTransform() { Rotation = rotation }; // 假设旋转后的方向为子弹的运动方向

            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.AddComponentData(projectileEntity, moveSpeed);
            entityManager.AddComponentData(projectileEntity, moveDirection);
        }
    }
}