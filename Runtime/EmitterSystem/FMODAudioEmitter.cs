using FMOD.Studio;
using FMODUnity;
using MazurkaGameKit.FMODTools;
using UnityEngine;


public abstract class FMODAudioEmitter : MonoBehaviour, IFMODAudioEmitter
{
    [SerializeField] protected GameObject overrrideSoundSource;
    [SerializeField] protected bool canBePaused = true;
    [SerializeField] protected bool canEmitSound = true;
    [SerializeField] protected bool stopSoundOnDestroy = true;

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

    public virtual GameObject GetSoundEmitter => overrrideSoundSource == null ? gameObject : overrrideSoundSource;

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

                canEmitSound = value;
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
        if(stopSoundOnDestroy)
            StopEmitter();

        UnregisterEmitter();
    }


    #region Utility

    public virtual bool PlayOneShot(EventReference eventRef)
    {
        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound_OneShot(eventRef, GetSoundEmitter))
        {
            OnSoundWasPlayed();
            return true;
        }

        return false;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef parameter)
    {
        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound_OneShot(eventRef, GetSoundEmitter, parameter))
        {
            OnSoundWasPlayed();
            return true;
        }

        return false;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef[] parameters)
    {
        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound_OneShot(eventRef, GetSoundEmitter, parameters))
        {
            OnSoundWasPlayed();
            return true;
        }

        return false;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef parameter, Vector3 atPos)
    {
        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound_OneShot(eventRef, atPos, parameter))
        {
            OnSoundWasPlayed();
            return true;
        }

        return false;
    }

    public virtual bool PlayOneShot(EventReference eventRef, ParamRef[] parameters, Vector3 atPos)
    {
        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound_OneShot(eventRef, atPos, parameters))
        {
            OnSoundWasPlayed();
            return true;
        }

        return false;
    }



    public virtual bool PlaySound(EventReference eventRef, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        if (FMODHelper.PlaySound(eventRef, GetSoundEmitter, out instance))
        {
            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);

            return true;
        }

        return false;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef parameter, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        if(FMODHelper.PlaySound(eventRef, GetSoundEmitter, parameter, out instance))
        {
            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);

            return true;
        }

        return false;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef[] parameters, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        if(FMODHelper.PlaySound(eventRef, GetSoundEmitter, parameters, out instance))
        {
            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);

            return true;
        }

        return false;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef parameter, Vector3 atPos, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

       if(FMODHelper.PlaySound(eventRef, atPos, parameter, out instance))
        {
            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);

            return true;
        }

        return false;
    }

    public virtual bool PlaySound(EventReference eventRef, ParamRef[] parameters, Vector3 atPos, out EventInstance instance)
    {
        instance = default;

        if (!CanPlayNewSound()) return false;

        if(FMODHelper.PlaySound(eventRef, atPos, parameters, out instance))
        {
            RegisterNewEventInstance(instance);
            OnSoundWasPlayed(instance);

            return true;
        }

        return false;
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


    public virtual void SetNewSourceTransform(GameObject source) => overrrideSoundSource = source;

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
