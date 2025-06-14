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

        public override void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}
