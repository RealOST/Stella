using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DodgeSystem : MonoBehaviour,IInjectable {
    public static event UnityAction OnDodgeStarted;
    public static event UnityAction OnDodgeEnded;

    private DodgeData dodgeData;

    private bool isDodging = false;
    private float currentRoll;
    private float direction;
    private Collider2D col;
    private Rigidbody2D rb;
    
    public void Inject(PlayerContext context) {
        dodgeData = context.DodgeData;
        rb = context.Rigidbody2D;
        col = context.Collider2D;
    }
    //
    // public void Init(Rigidbody2D rb, Collider2D col) {
    //     this.col = col;
    //     this.rb = rb;
    // }

    public void Dodge() {
        if (isDodging) return;
        StartCoroutine(DodgeCoroutine());
    }

    private IEnumerator DodgeCoroutine() {
        isDodging = true;
        col.isTrigger = true;
        OnDodgeStarted?.Invoke();

        AudioManager.Instance.PlayRandomSFX(dodgeData.dodgeSFX);

        yield return DoDodgeMotion();

        isDodging = false;
        col.isTrigger = false;
        OnDodgeEnded?.Invoke();
    }

    private IEnumerator DoDodgeMotion() {
        currentRoll = 0f;
        direction = rb.velocity.y >= 0 ? 1 : -1;

        while (currentRoll < dodgeData.maxRoll) {
            currentRoll += dodgeData.rollSpeed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(currentRoll * direction, Vector3.right);
            transform.localScale =
                BezierCurve.QuadraticPoint(Vector3.one, Vector3.one, dodgeData.dodgeScale, currentRoll / dodgeData.maxRoll);
            yield return null;
        }
    }

}