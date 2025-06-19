using UnityEngine;

namespace UI.GameView
{
    public abstract class View : MonoBehaviour
    {
        public abstract void Init();
        public abstract void OnMainGuiDestroy();

        public virtual void OnMainUIStart()
        {
            
        }
    }
}
