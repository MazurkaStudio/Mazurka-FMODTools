using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazurkaGameKit.FMODTools
{
    /// <summary>
    /// This class allow you to play on shots events on UI events
    /// </summary>
    public class FMODUIEventsController : MonoBehaviour, 
        ISubmitHandler, ICancelHandler, 
        ISelectHandler, IDeselectHandler, 
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private FMODAudioEmitter audioEmitter;

        [SerializeField] private EventReference onEnable, onDisable;
        private void OnEnable()
        {
            if(!onEnable.IsNull) audioEmitter.PlayOneShot(onEnable);
        }
        private void OnDisable()
        {
            if(!onDisable.IsNull) audioEmitter.PlayOneShot(onDisable);
        }
        

        [SerializeField] private EventReference onSubmit;
        public virtual void OnSubmit(BaseEventData eventData)
        {
            if(!onSubmit.IsNull) audioEmitter.PlayOneShot(onSubmit);
        }
        
        [SerializeField] private EventReference onCancel;
        public virtual void OnCancel(BaseEventData eventData)        
        {
            if(!onCancel.IsNull) audioEmitter.PlayOneShot(onCancel);
        }

        [SerializeField] private EventReference onSelect;
        public virtual void OnSelect(BaseEventData eventData)        
        {
            if(!onSelect.IsNull) audioEmitter.PlayOneShot(onSelect);
        }
        
        [SerializeField] private EventReference onDeselect;
        public virtual void OnDeselect(BaseEventData eventData)        
        {
            if(!onDeselect.IsNull) audioEmitter.PlayOneShot(onDeselect);
        }
        
        [SerializeField] private EventReference onPointerEnter;
        public virtual void OnPointerEnter(PointerEventData eventData)        
        {
            if(!onPointerEnter.IsNull) audioEmitter.PlayOneShot(onPointerEnter);
        }

        [SerializeField] private EventReference onPointerExit;
        public virtual void OnPointerExit(PointerEventData eventData)        
        {
            if(!onPointerExit.IsNull) audioEmitter.PlayOneShot(onPointerExit);
        }
        
        [SerializeField] private EventReference onPointerClick;
        public virtual void OnPointerClick(PointerEventData eventData)        
        {
            if(!onPointerClick.IsNull) audioEmitter.PlayOneShot(onPointerClick);
        }
    }
}

