namespace UI.GameView
{
    public interface IEndGameViewController 
    {
        void ShowEndGameView(string congratulationsYouHaveCompletedAllWaves);
        void OnRestartClicked();
        void OnQuitClicked();
    }
}
