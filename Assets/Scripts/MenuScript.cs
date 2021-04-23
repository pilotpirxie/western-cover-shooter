using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private bool isMenuShown;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject tip;
    [SerializeField] private GameObject continueText;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuSound;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isMenuShown)
            {
                audioSource.PlayOneShot(menuSound);
                fadeIn.SetActive(true);
                Invoke("LoadGameplay", 4f);
            }
            else
            {
                isMenuShown = true;
                background.SetActive(isMenuShown);
                tip.SetActive(isMenuShown);
                continueText.SetActive(isMenuShown);
                audioSource.PlayOneShot(menuSound);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isMenuShown = false;
            background.SetActive(isMenuShown);
            tip.SetActive(isMenuShown);
            continueText.SetActive(isMenuShown);
            audioSource.PlayOneShot(menuSound);
        }
    }

    void LoadGameplay()
    {
        SceneManager.LoadScene("Cover");
    }
}