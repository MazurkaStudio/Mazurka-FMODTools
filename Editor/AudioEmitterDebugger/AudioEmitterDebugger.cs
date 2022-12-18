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
          window.maxSize = new Vector2(500f, 500f);
          window.minSize = window.maxSize;
      }
  
      private void OnEnable()
      {
          ActiveEmitters = new List<IFMODAudioEmitter>();
          GetTexture();
      }
  
      public List<IFMODAudioEmitter> ActiveEmitters;
      private Texture simpleEmitterTex, complexEmitterTex;
      private Texture muteEmitterTex, nonMuteEmitterTex;
      private Texture playTex, pauseTex, stopTex;

      private void GetTexture()
      {
          simpleEmitterTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/SimpleEmitterIcon");
          complexEmitterTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/EmitterIcon");
          muteEmitterTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/MuteEmitter");
          nonMuteEmitterTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/NonMuteEmitter");
          playTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/Play");
          pauseTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/Pause");
          stopTex = Resources.Load<Texture>("MazurkaGameKit/FMODTools/Icons/Stop");
      }

      private Vector2 scrollPos;
      
      private void OnGUI()
      {
          scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
          
          EditorGUILayout.BeginVertical("box");

          totalPlaying = 0;
          totalSimple = 0;
          totalComplex = 0;
          
          if (Application.isPlaying)
          {
              ActiveEmitters = FMODAudioEmitterManager.allActiveEmitters;
              total = ActiveEmitters.Count;
              for (int i = 0; i < total; i++)
              {
                  IFMODAudioEmitter emitter = ActiveEmitters[i];
                  DrawEmitter(emitter);
              }

            
          }
          else
          {
              FMODAudioEmitter[] emitters = FindObjectsOfType<FMODAudioEmitter>();
              total = emitters.Length;
              for (int i = 0; i < total; i++)
              {
                  IFMODAudioEmitter emitter = emitters[i];
                  DrawEmitter(emitter);
              }
          }

          EditorGUILayout.EndVertical();
          
          EditorGUILayout.Space(30f);
          
          DrawResults();
          
          EditorGUILayout.EndScrollView();
      }

      private void DrawEmitter(IFMODAudioEmitter emitter)
      {
          EditorGUILayout.BeginHorizontal();
              
          EditorGUILayout.LabelField(emitter.GetSoundEmitter.name, GUILayout.Width(200f));
          if (emitter is FMODComplexAudioEmitter)
          {
              totalComplex++;
              GUILayout.Box(complexEmitterTex, GUILayout.Height(20), GUILayout.Width(20));
          }
          else
          {
              totalSimple++;
              GUILayout.Box(simpleEmitterTex, GUILayout.Height(20), GUILayout.Width(20));
          }
          
          EditorGUILayout.Space(10f);
          
          GUILayout.Box(emitter.CanEmitSound ? nonMuteEmitterTex : muteEmitterTex, GUILayout.Height(20), GUILayout.Width(20));
          EditorGUILayout.Space(10f);
          if (emitter.IsInPause)
          {
              GUILayout.Box(pauseTex, GUILayout.Height(20), GUILayout.Width(20));
          }
          else
          {
              if (emitter.IsPlaying)
              {
                  totalPlaying++;
                  GUILayout.Box(playTex, GUILayout.Height(20), GUILayout.Width(20));
              }
              else
              {
                  GUILayout.Box(stopTex, GUILayout.Height(20), GUILayout.Width(20));
              }
             
          }
          EditorGUILayout.Space(10f);
          if (GUILayout.Button("Select", GUILayout.Width(100f)))
          {
              FocusObject(emitter.GetSoundEmitter);
          }
              
          EditorGUILayout.EndHorizontal();
          
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(15));
      }

      private int total;
      private int totalPlaying;
      private int totalSimple;
      private int totalComplex;
      
      private void DrawResults()
      {
          EditorGUILayout.BeginVertical("box");
          
          EditorGUILayout.LabelField("Total = " + total);
          EditorGUILayout.LabelField("Total Playing = " + totalPlaying);
          EditorGUILayout.LabelField("Total Simple = " + totalSimple);
          EditorGUILayout.LabelField("Total Complex = " + totalComplex);
          
          EditorGUILayout.EndVertical();
      }
      
      public static void FocusObject(GameObject gmaeObject)
      {
          Selection.activeGameObject = gmaeObject;
          SceneView.FrameLastActiveSceneView();
      }
  }
  
}

