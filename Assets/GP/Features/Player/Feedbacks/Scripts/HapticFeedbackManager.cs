#if UNITY_STANDALONE
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class HapticFeedbackManager : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Vibration hapticAction;

    [SerializeField] [FoldoutGroup("Player Weapon Variables")] private float gunshotDuration, gunshotFrequency, gunshotAmplitude;
    [SerializeField] [FoldoutGroup("Player Weapon Variables")] private SteamVR_Input_Sources weaponHand;

    [SerializeField] [FoldoutGroup("Player Reload Variables")] private float reloadDuration, reloadFrequency, reloadAmplitude;
    [SerializeField] [FoldoutGroup("Player Reload Variables")] private SteamVR_Input_Sources reloadHand;

    [SerializeField] [FoldoutGroup("UI Click Variables")] private float uIClickDuration, uIClickFrequency, uIClickAmplitude;
    [SerializeField] [FoldoutGroup("UI Click Variables")] private SteamVR_Input_Sources uIClickHand;

    [SerializeField] [FoldoutGroup("UI Hover Variables")] private float uIHoverDuration, uIHoverFrequency, uIHoverAmplitude;
    [SerializeField] [FoldoutGroup("UI Hover Variables")] private SteamVR_Input_Sources uIHoverHand;

    [SerializeField] [FoldoutGroup("Testing Variables")] private bool isTesting = false;
    [SerializeField] [FoldoutGroup("Testing Variables")] private float testDuration, testFrequency, testAmplitude;
    [SerializeField] [FoldoutGroup("Testing Variables")] private SteamVR_Input_Sources testHand;

    private void Start()
    {
        if (isTesting) StartCoroutine(LoopPulse());
    }

    private IEnumerator LoopPulse()
    {
        Pulse(testDuration, testFrequency, testAmplitude, testHand);

        new WaitForSeconds(1.5f);

        yield return null;

        StartCoroutine(LoopPulse());
    }

    public void GE_Gunshot()
    {
        Pulse(gunshotDuration, gunshotFrequency, gunshotAmplitude, weaponHand);
    }

    public void GE_Reload()
    {
        Pulse(reloadDuration, reloadFrequency, reloadAmplitude, reloadHand);
    }

    public void GE_UIClick()
    {
        Pulse(uIClickDuration, uIClickFrequency, uIClickAmplitude, uIClickHand);
    }

    public void GE_UIHover()
    {
        Pulse(uIHoverDuration, uIHoverFrequency, uIHoverAmplitude, uIHoverHand);
    }

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources inputSource)
    {
        if (XRSettings.enabled) hapticAction.Execute(0, duration, frequency, amplitude, inputSource);
    }
}
#endif