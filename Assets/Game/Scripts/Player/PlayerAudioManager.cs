using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _footstepSFX;
    [SerializeField] private AudioSource _punchSFX;
    [SerializeField] private AudioSource _glideSFX;
    [SerializeField] private AudioSource _landingSFX;

    [Header("Sfx")]
    [SerializeField] private float _resetComboInterval;
    private void Start()
    {
        InputEventManager.OnSprintInput += IsSprinting;
        PlayerEventManager.OnAudioGliding += GlideSFX;
        PlayerEventManager.OnAnimationPunch += CountPunch;
    }

    private void OnDestroy()
    {
        InputEventManager.OnSprintInput -= IsSprinting;
        PlayerEventManager.OnAudioGliding -= GlideSFX;
        PlayerEventManager.OnAnimationPunch -= CountPunch;
    }

    private bool _isSprint;

    private void IsSprinting(bool _sprintInput)
    {
        _isSprint = _sprintInput;
    }
    
    private void PlayFootstepSFX()
    {
        if (_isSprint)
        {
            _footstepSFX.volume = Random.Range(0.55f, 0.8f);
            _footstepSFX.pitch = Random.Range(1.5f, 2.5f);
            _footstepSFX.Play();
        }
        else
        {
            _footstepSFX.volume = Random.Range(0.3f, 0.55f);
            _footstepSFX.pitch = Random.Range(0.5f, 1f);
            _footstepSFX.Play();
        }
    }

    private int _combo = 0;

    private void CountPunch(int _comboSFX)
    {
        _combo = _comboSFX;
    }
    
    private void PlayPunchSFX()
    {
        if (_combo == 1)
        {
            _punchSFX.pitch = 1f;
            _punchSFX.Play();
        }
        else if (_combo == 2)
        {
            _punchSFX.pitch = 0.7f;
            _punchSFX.Play();
        }
        else if (_combo == 3)
        {
            _punchSFX.pitch = 0.5f;
            _punchSFX.Play();
        }
        else if (_combo == 4)
        {
            _punchSFX.pitch = 0.7f;
            _punchSFX.Play();
        }
        else if (_combo == 5)
        {
            _punchSFX.pitch = 0.5f;
            _punchSFX.Play();
        }
    }

    private void GlideSFX(bool _checkGlideSFX)
    {
        if (_checkGlideSFX)
        {
            _glideSFX.Play();
        }
        else
        {
            _glideSFX.Stop();
        }
    }
    
    private void PlayLandingSFX()
    {
        _landingSFX.Play();
    }
}
