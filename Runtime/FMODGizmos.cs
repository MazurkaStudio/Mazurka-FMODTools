using FMOD.Studio;
using FMODUnity;
using MazurkaGameKit.FMODTools;
using Newtonsoft.Json.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazurkaGameKit.FMODTools
{
    public static class FMODGizmos
    {
        public static void VisuzalizeSound3DAttributs(EventReference eventRef, Transform soundSource)
        {
            if (FMODHelper.GetEventDescription(eventRef).getMinMaxDistance(out float min, out float max) == FMOD.RESULT.OK)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(soundSource.position, min);
                Gizmos.DrawWireSphere(soundSource.position, max);
            }

        }
    }
}
