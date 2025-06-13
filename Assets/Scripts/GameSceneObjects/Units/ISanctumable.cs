namespace GameSceneObjects.Units
{
    public interface ISanctumable 
    {
        void Heal(float healEffect);
        void OnSanctumeEnter();
        void OnSanctumExit();
        bool IsSanctumActive(); //TODO: check if make it as a property would be better
    }
}
