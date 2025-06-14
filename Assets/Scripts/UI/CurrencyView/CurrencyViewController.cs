using Currencies;
using Services;
using UnityEngine;

namespace UI.CurrencyView
{
    public class CurrencyViewController : MonoBehaviour, ICurrencyController
    {
        private ICurrencyTracker currencyTracker;
        private ICurrencyView currencyView;

        private void Awake()
        {
            ServiceLocator.Instance.Register<ICurrencyController>(this);
        }

        private void Start()
        {
            currencyTracker = ServiceLocator.Instance.Get<ICurrencyTracker>();
            currencyView = ServiceLocator.Instance.Get<ICurrencyView>();

            currencyTracker.AddListener(OnCurrencyChanged);
        }

        private void OnCurrencyChanged(CurrencyEventParams obj)
        {
            currencyView.SetCurrency(obj.NewValue);
        }

        private void OnDestroy()
        {
            currencyTracker.RemoveListener(OnCurrencyChanged);
        }
    }
}
