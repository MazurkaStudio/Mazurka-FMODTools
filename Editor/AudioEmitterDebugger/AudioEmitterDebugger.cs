using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MazurkaGameKit.FMODTools;
using UnityEngine;
using UnityEditor;

namespace MazurkaGameKit.FMODTools.Editor
{
  public class AudioEmitterDebugger : EditorWindow
  {
      [MenuItem("MazurkaGameKit/FMOD Tools/Audio Emitter Debugger")]
      public static void CreateWindow()
      {
          AudioEmitterDebugger window = (AudioEmitterDebugger)GetWindow(typeof(AudioEmitterDebugger));
          window.titleContent = new GUIContent("Audio Emitter Debugger");
      }
  
      private void OnEnable()
      {
          ActiveEmitters = new List<FMODAudioEmitter>();
          Refresh();
      }
  
      public List<FMODAudioEmitter> ActiveEmitters;
  
      public static void Refresh()
      {
          AudioEmitterDebugger window = (AudioEmitterDebugger)GetWindow(typeof(AudioEmitterDebugger));
          window.ActiveEmitters = FindObjectsOfType<FMODAudioEmitter>().ToList();
      }
  
      private void OnGUI()
      {
          EditorGUILayout.BeginVertical("box");
          
          for (int i = 0; i < ActiveEmitters.Count; i++)
          {
              FMODAudioEmitter emitter = ActiveEmitters[i];
              
              EditorGUILayout.BeginHorizontal("box");
              
              EditorGUILayout.LabelField(emitter.name);
              EditorGUILayout.LabelField(emitter is FMODComplexAudioEmitter ? "Complex" : "Simple");
              
              EditorGUILayout.LabelField(emitter.CanEmitSound ? "Can play" : "Is muted");
              EditorGUILayout.LabelField(emitter.CanBePaused ? "Can be paused" : "Can not be paused");
              
              if (emitter.IsInPause)
              {
                  EditorGUILayout.LabelField(emitter.IsPlaying ? "Is paused" : "Is stop");
              }
              else
              {
                  EditorGUILayout.LabelField(emitter.IsPlaying ? "Is playing" : "Is stop");
              }
              
              if (GUILayout.Button("Select"))
              {
                  FocusObject(ActiveEmitters[i].gameObject);
              }
              
              EditorGUILayout.EndHorizontal();
          }
          
          EditorGUILayout.EndVertical();
      }
      
      public static void FocusObject(GameObject gmaeObject)
      {
          Selection.activeGameObject = gmaeObject;
          SceneView.FrameLastActiveSceneView();
      }
  }
  
}

