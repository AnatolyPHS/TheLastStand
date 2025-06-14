using GameEvents;

namespace Currencies
{
    public interface ICurrencyTracker : IEventTrigger<CurrencyEvent, CurrencyEventParams>
    {
        void ChangeCurrency(int delta);
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
