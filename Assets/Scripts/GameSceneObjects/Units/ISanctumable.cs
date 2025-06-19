namespace GameSceneObjects.Units
{
    public interface ISanctumable 
    {
        void Heal(float healEffect);
        void OnSanctumeEnter();
        void OnSanctumExit();
    }
}
