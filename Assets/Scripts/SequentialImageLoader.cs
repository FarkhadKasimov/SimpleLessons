using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequentialImageLoader : MonoBehaviour
{
    public string url = "https://picsum.photos/200/300";
    public float delay = 1f;

    private List<Texture2D> textures = new List<Texture2D>();
    private int currentIndex = 0;

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
            }
        }

        StartCoroutine(DisplayTextures());
    }

    IEnumerator DisplayTextures()
    {
        while (currentIndex < textures.Count)
        {
            GameObject imageObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            imageObject.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            imageObject.GetComponent<Renderer>().material.mainTexture = textures[currentIndex];

            currentIndex++;

            yield return new WaitForSeconds(delay);
        }
    }
}