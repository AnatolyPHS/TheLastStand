using EffectsManager;
using GameSceneObjects.HeroManagement;
using GameSceneObjects.StateBehaviour.HeroStates;
using GameSceneObjects.Units;

namespace GameSceneObjects.StateBehaviour
{
    public class HeroStationBehaviour : StationBehaviour
    {
        public HeroStationBehaviour(Hero hero, IHeroManager heroManager, IEffectHolder effectHolder)
        {
            currentUnitState = new HeroCalmUnitState(hero, this);
            states.Add(currentUnitState);
            states.Add(new HeroAttackTargetUnitState(hero, effectHolder, this));
            states.Add(new HeroMoveToTargetUnitState(hero, this));
            states.Add(new HeroMoveToPointUnitState(hero, this));
        }
    }
}
