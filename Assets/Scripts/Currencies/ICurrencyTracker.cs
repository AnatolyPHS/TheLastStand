using GameEvents;

namespace Currencies
{
    public interface ICurrencyTracker : IEventTrigger<CurrencyEvent, CurrencyEventParams>
    {
        void ChangeCurrencyValue(int delta);
        int CurrencyValue { get; }
    }

    public struct CurrencyEventParams
    {
        public int OldValue;
        public int NewValue;
    }

    public class CurrencyEvent : BaseEvent
    {
    }
}
