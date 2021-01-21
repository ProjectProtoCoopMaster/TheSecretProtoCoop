using UnityEngine;

public class VR_GunReloading : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void GE_StartReload()
    {
        animator.SetBool("isPulling", true);
    }

    public void GE_EndReload()
    {
        animator.SetBool("isPulling", false);
    }
}
