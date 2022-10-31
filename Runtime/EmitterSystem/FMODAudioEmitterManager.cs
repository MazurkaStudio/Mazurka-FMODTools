using MazurkaGameKit.FMODTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    public static class FMODAudioEmitterManager
    {
        public static List<IFMODAudioEmitter> allActiveEmitters = new List<IFMODAudioEmitter>();

        public static void Register(IFMODAudioEmitter emitter)
        {
            if (allActiveEmitters.Contains(emitter))
                return;

            allActiveEmitters.Add(emitter);
        }

        public static void Unregister(IFMODAudioEmitter emitter)
        {
            if (!allActiveEmitters.Contains(emitter))
                return;

            allActiveEmitters.Remove(emitter);
        }

        public static void PauseAllEmitters(bool value)
        {
            foreach (IFMODAudioEmitter emitter in allActiveEmitters)
            {
                emitter.PauseAllEventInstance(value);
            }
        }

        public static void StopAllEmittters()
        {
            foreach (IFMODAudioEmitter emitter in allActiveEmitters)
            {
                emitter.StopAllEventInstance();
            }
        }
    }

}

