using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public CinemachineBasicMultiChannelPerlin channel;
    public NoiseSettings settingsStart;
    public float amplitudeGainStart;
    public float m_FrequencyGainStart;
    public float amplitudeGainPlayerHit;
    public float m_FrequencyGainPlayerHit;
    public float playerHitDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        channel = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        settingsStart = channel.m_NoiseProfile;
        amplitudeGainStart = channel.m_AmplitudeGain;
        m_FrequencyGainStart = channel.m_FrequencyGain;
    }

    public void PlayerHitShake()
    {
        StopCoroutine("PlayerHitShakeCo");
        StartCoroutine("PlayerHitShakeCo");
    }

    IEnumerator PlayerHitShakeCo()
    {
        channel.m_AmplitudeGain = amplitudeGainPlayerHit;
        channel.m_FrequencyGain = m_FrequencyGainPlayerHit;
        yield return new WaitForSeconds(playerHitDuration);
        channel.m_AmplitudeGain = amplitudeGainStart;
        channel.m_FrequencyGain = m_FrequencyGainStart;
        yield return null;
    }
}
