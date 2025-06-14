using System;
using Services;
using UnityEngine;
using GameEvents;

namespace Currencies
{
    public class CurrencyTracker : MonoBehaviour, ICurrencyTracker
    {
        [SerializeField] private int initialCurrencyValue = 200;
        
        private EventTrigger eventTrigger;
        private int currencyValue;
        
        public int CurrencyValue => currencyValue;
        
        private void Awake()
        {
            currencyValue = initialCurrencyValue;
            
            ServiceLocator.Instance.Register<ICurrencyTracker>(this);
            
            eventTrigger = new EventTrigger();
        }
        
        public void AddListener(Action<CurrencyEventParams> listener)
        {
            eventTrigger.AddListener<CurrencyEvent, CurrencyEventParams>(listener);
        }

        public void RemoveListener(Action<CurrencyEventParams> listener)
        {
            eventTrigger.RemoveListener<CurrencyEvent, CurrencyEventParams>(listener);
        }

        public void ChangeCurrencyValue(int delta)
        {
            int oldValue = currencyValue;
            currencyValue += delta;
            currencyValue = currencyValue <= 0 ? currencyValue = 0 : currencyValue;
            
            if (oldValue != currencyValue)
            {
                eventTrigger.RaiseEvent<CurrencyEvent, CurrencyEventParams>(new CurrencyEventParams
                {
                    OldValue = oldValue,
                    NewValue = currencyValue
                });                
            }
        }
    }
}
