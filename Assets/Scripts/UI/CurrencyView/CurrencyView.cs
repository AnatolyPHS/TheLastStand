using Services;
using TMPro;
using UI.GameView;
using UnityEngine;

namespace UI.CurrencyView
{
    public class CurrencyView : View, ICurrencyView
    {
        [SerializeField] private TextMeshProUGUI currencyText;
    
        public void SetCurrency(int value)
        {
            currencyText.text = value.ToString();
        }

        public void SetActiveState(bool state)
        {
            gameObject.SetActive(state);
        }

        public override void Init()
        {
            ServiceLocator.Instance.Register<ICurrencyView>(this);
        }
    }
}
