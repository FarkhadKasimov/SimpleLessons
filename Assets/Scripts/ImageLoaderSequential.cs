using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoaderSequential : MonoBehaviour
{
    public string url = "https://picsum.photos/200/300";
    public List<Image> images;

    private List<Texture2D> textures = new List<Texture2D>();

    IEnumerator Start()
    {
        for (int i = 0; i < 6; i++)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url + "?random=" + Random.Range(0, 100000));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                textures.Add(texture);

                if (textures[i] != null && images[i] != null)
                {
                    images[i].sprite = Sprite.Create(textures[i], new Rect(0, 0, textures[i].width, textures[i].height), Vector2.zero);
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}