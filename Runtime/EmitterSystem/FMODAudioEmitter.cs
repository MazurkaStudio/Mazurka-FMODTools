using FMOD.Studio;
using FMODUnity;
using MazurkaGameKit.FMODTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public abstract class FMODAudioEmitter : MonoBehaviour, IFMODAudioEmitter
{
    [SerializeField] protected Transform overrrideSoundSource;
    [SerializeField] protected bool canBePaused = true;
    [SerializeField] protected bool canEmitSound = true;

    #region Interface

    #region Emitter Registration

    public abstract void RegisterEmitter();

    public abstract void UnregisterEmitter();

    #endregion

    #region Event Instances Registration


    public abstract void RegisterNewEventInstance(EventInstance eventInstance);

    public abstract void UnregisterNewEventInstance(EventInstance eventInstance);

    public abstract void RefreshEventsInstances();

    #endregion

    public virtual Transform GetSoundEmitter => overrrideSoundSource == null ? transform : overrrideSoundSource;

    public virtual bool IsInPause => isInPause;

    public virtual bool CanEmitSound
    {
        get
        {
            return canEmitSound;
        }
        set
        {
            if (canEmitSound != value)
            {
                if (!value)
                {
                    StopEmitter();
                }
            }
        }
    }

    protected bool isInPause = false;

    public abstract void PauseEmitter(bool value);

    public abstract void StopEmitter(bool allowFadeOut = true);

    #endregion

    public virtual bool CanPlayNewSound()
    {
        if (!canEmitSound || isInPause)
            return false;

        return true;
    }

    protected virtual void Start()
    {
        RegisterEmitter();
    }

    protected virtual void OnDestroy()
    {
        StopEmitter();
        UnregisterEmitter();
    }


    #region Utility

    public virtual bool PlayOneShot(EventReference eventRef)
    {
        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound_OneShot(eventRef, gameObject);

        OnSoundWasPlayed();

        return true;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef parameter)
    {
        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound_OneShot(eventRef, gameObject, parameter);

        OnSoundWasPlayed();

        return true;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef[] parameters)
    {
        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound_OneShot(eventRef, gameObject, parameters);

        OnSoundWasPlayed();

        return true;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef parameter, Vector3 atPos)
    {
        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound_OneShot(eventRef, atPos, parameter);

        OnSoundWasPlayed();

        return true;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef[] parameters, Vector3 atPos)
    {
        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound_OneShot(eventRef, atPos, parameters);

        OnSoundWasPlayed();

        return true;
    }



    public virtual bool PlaySound(EventReference eventRef, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound(eventRef, gameObject);

        RegisterNewEventInstance(instance);
        OnSoundWasPlayed(instance);

        return true;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef parameter, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound(eventRef, gameObject, parameter);

        RegisterNewEventInstance(instance);
        OnSoundWasPlayed(instance);

        return true;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef[] parameters, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound(eventRef, gameObject, parameters);

        RegisterNewEventInstance(instance);
        OnSoundWasPlayed(instance);

        return true;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef parameter, Vector3 atPos, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound(eventRef, atPos, parameter);

        RegisterNewEventInstance(instance);
        OnSoundWasPlayed(instance);

        return true;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef[] parameters, Vector3 atPos, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        FMODHelper.PlaySound(eventRef, atPos, parameters);

        RegisterNewEventInstance(instance);
        OnSoundWasPlayed(instance);

        return true;
    }



    public virtual void SetParameter(EventInstance eventInstance, ParamRef parameter)
    {
        if (eventInstance.isValid())
            eventInstance.setParameterByName(parameter.Name, parameter.Value);
    }

    public virtual void SetParameter(EventInstance eventInstance, ParamRef[] parameters)
    {
        if (eventInstance.isValid())
        {
            foreach (ParamRef parameter in parameters)
            {
                eventInstance.setParameterByName(parameter.Name, parameter.Value);
            }
        }
    }



    public virtual void StopSound(EventInstance instance, FMOD.Studio.STOP_MODE stopMode = FMOD.Studio.STOP_MODE.ALLOWFADEOUT)
    {
        if (!instance.isValid())
            return;

        UnregisterNewEventInstance(instance);
        OnSoundWillStopped(instance);
        instance.stop(stopMode);
    }

    public virtual void PauseSound(EventInstance instance, bool value)
    {
        if (!instance.isValid())
            return;

        isInPause = value;
        OnSoundWillPaused(instance);
        instance.setPaused(value);
    }

    #endregion


    #region Callbacks

    public virtual void OnSoundWasPlayed()
    {

    }

    public virtual void OnSoundWasPlayed(EventInstance instance)
    {

    }

    public virtual void OnSoundWillStopped(EventInstance instance)
    {

    }

    public virtual void OnSoundWillPaused(EventInstance instance)
    {

    }

    public virtual void OnSoundWasResumed(EventInstance instance)
    {

    }

    #endregion
}
