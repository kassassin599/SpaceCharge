using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;  // Singleton instance
  public GameObject[] storyPanels;     // Array of story panels
  private int currentPanelIndex = 0;   // Current story panel being displayed

  private bool isStoryPlaying = true;  // Flag to track if the story is playing

  public TextMeshProUGUI clickContinueText;
  public GameObject[] gameplayGO;
  public GameObject startPanelGO;
  public GameObject endPanelGO;
  public GameObject storyPanelGO;

  private void Awake()
  {
    // Singleton pattern to ensure only one instance of GameManager
    if (instance == null)
    {
      instance = this;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    DisableGO();
    isStoryPlaying = false;
  }

  public void DisableGO()
  {
    foreach (GameObject go in gameplayGO)
    {
      go.SetActive(false);
    }
  }

  void Update()
  {
    if (isStoryPlaying && Input.GetMouseButtonDown(0))
    {
      ShowNextPanel();
    }
  }

  // Pauses the gameplay
  public void PauseGameplay()
  {
    Time.timeScale = 0f;  // Freezes all gameplay
                          // Optionally disable player/enemy scripts if needed

    
  }

  // Resumes the gameplay
  public void ResumeGameplay()
  {
    Time.timeScale = 1f;  // Resumes all gameplay
                          // Optionally re-enable player/enemy scripts

    clickContinueText.gameObject.SetActive(false);
    EnableGO();
  }

  public void EnableGO()
  {
    foreach (GameObject go in gameplayGO)
    {
      go.SetActive(true);
    }
  }

  // Shows a specific story panel and animates it upwards
  void ShowStoryPanel(int index)
  {
    if (index < storyPanels.Length)
    {
      storyPanels[index].SetActive(true);
      AnimatePanelUpwards(storyPanels[index]);  // Play animation
    }
  }

  // Animates the panel upwards
  void AnimatePanelUpwards(GameObject panel)
  {
    Animator animator = panel.GetComponent<Animator>();
    if (animator != null)
    {
      animator.SetTrigger("MoveUp");
    }
  }

  // Shows the next story panel
  void ShowNextPanel()
  {
    // Deactivate the current panel
    storyPanels[currentPanelIndex].SetActive(false);

    // Move to the next panel
    currentPanelIndex++;

    if (currentPanelIndex < storyPanels.Length)
    {
      ShowStoryPanel(currentPanelIndex);
    }
    else
    {
      EndStory();
    }
  }

  // Ends the story and resumes the gameplay
  void EndStory()
  {
    isStoryPlaying = false;
    ResumeGameplay();
  }

  public void PlayButton()
  {
    PauseGameplay();
    ShowStoryPanel(currentPanelIndex);
    isStoryPlaying = true;

    startPanelGO.SetActive(false);
  }

  public void RestartButton()
  {
    string currentSceneName = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(currentSceneName);
  }

  public void QuitButton()
  {
    Application.Quit();
  }
}
