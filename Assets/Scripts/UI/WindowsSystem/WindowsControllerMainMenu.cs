
using UnityEngine.SceneManagement;

public class WindowsControllerMainMenu : WindowsController
{

    public void StartNewGame()
    {
        PersistentDataManager.CreateInitialData();
        SceneManager.LoadScene(1);
    }
}
