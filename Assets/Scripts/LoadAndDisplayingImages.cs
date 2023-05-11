using DG.Tweening;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadAndDisplayingImages : MonoBehaviour
{
    public Image[] Images;

    [SerializeField] private Button m_LoadButton;

    [Header("Error Panel")]
    [SerializeField] private Button m_CloseErrorPanelButton;
    [SerializeField] private Image m_ErrorPanel;

    private int m_NumImagesDownloaded = 0;

    private void Start()
    {
        ClearAllImages();

        m_CloseErrorPanelButton.onClick.AddListener(CloseErrorPanel);
    }

    #region Load image and show when ready
    public void WhenImageReady()
    {
        ClearAllImages();
        for (int i = 0; i < Images.Length; i++)
        {
            StartCoroutine(ShowWhenReady(i));
        }
    }

    IEnumerator ShowWhenReady(int imageIndex)
    {
        m_LoadButton.interactable = false;

        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            Images[imageIndex].sprite = sprite;
            Images[imageIndex].transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
        }
        else
        {
            Debug.LogError(request.error);
            ShowErrorPanel();
        }

        m_NumImagesDownloaded++;
        if (m_NumImagesDownloaded == Images.Length)
        {
            m_LoadButton.interactable = true;
        }

        request.Dispose();
    }
    #endregion

    #region Parallel download and display images
    public void OneByOne()
    {
        ClearAllImages();
        StartCoroutine(Parallel());
    }

    IEnumerator Parallel()
    {
        m_LoadButton.interactable = false;

        foreach (Image image in Images)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                image.sprite = sprite;
                image.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
            }
            else
            {
                Debug.LogError(request.error);
                ShowErrorPanel();
            }

            request.Dispose();
        }

        m_LoadButton.interactable = true;
    }
    #endregion

    #region Downloading images and displaying after downloading all images
    public void AllAtOnce()
    {
        ClearAllImages();
        StartCoroutine(DisplayingAfterDownloading());
    }

    IEnumerator DisplayingAfterDownloading()
    {
        m_LoadButton.interactable = false;

        Texture2D[] textures = new Texture2D[Images.Length];
        UnityWebRequest[] requests = new UnityWebRequest[Images.Length];

        for (int i = 0; i < Images.Length; i++)
        {
            requests[i] = UnityWebRequestTexture.GetTexture("https://picsum.photos/200/300");
            yield return requests[i].SendWebRequest();

            if (requests[i].result == UnityWebRequest.Result.Success)
            {
                textures[i] = DownloadHandlerTexture.GetContent(requests[i]);
                m_NumImagesDownloaded++;

                if (m_NumImagesDownloaded == Images.Length)
                {
                    DisplayImages(textures);
                    m_LoadButton.interactable = true;
                }
            }
            else
            {
                Debug.LogError(requests[i].error);
                ShowErrorPanel();
            }

            requests[i].Dispose();
        }
    }

    private void DisplayImages(Texture2D[] textures)
    {
        foreach (Image image in Images)
        {
            int index = System.Array.IndexOf(Images, image);
            Sprite sprite = Sprite.Create(textures[index], new Rect(0, 0, textures[index].width, textures[index].height), Vector2.zero);
            image.sprite = sprite;
            image.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
        }
    }
    #endregion

    void ClearAllImages()
    {
        m_NumImagesDownloaded = 0;
        for (int i = 0; i < Images.Length; i++)
        {
            Images[i].transform.DOScale(0f, 0.1f).SetEase(Ease.Linear);
        }
        Caching.ClearCache();
    }

    void ShowErrorPanel()
    {
        StopAllCoroutines();
        ClearAllImages();
        m_ErrorPanel.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
    }

    void CloseErrorPanel()
    {
        m_LoadButton.interactable = true;
        m_ErrorPanel.transform.DOScale(0f, 0.1f).SetEase(Ease.Linear);
    }
}