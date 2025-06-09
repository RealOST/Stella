using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Data/Weapon Data/Weapon Pattern")]
public class WeaponPatternData : ScriptableObject {
    [System.Serializable]
    public struct FirePoint {
        public int projectileIndex;
        public int muzzleIndex;
    }

    public FirePoint[] firePoints;

    public void Execute(Transform[] muzzles, UnityAction<int, Transform> fireAction) {
        foreach (var fp in firePoints) {
            if (fp.muzzleIndex < muzzles.Length)
                fireAction.Invoke(fp.projectileIndex, muzzles[fp.muzzleIndex]);
        }
    }
}
