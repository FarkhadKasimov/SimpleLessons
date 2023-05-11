using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ParralelImageLoader : MonoBehaviour
{
    public string url = "https://picsum.photos/200/300";
    public float delay = 1f;

    private List<Texture2D> textures = new List<Texture2D>();
    private List<GameObject> imageObjects = new List<GameObject>();

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

                GameObject imageObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                imageObject.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                imageObject.GetComponent<Renderer>().material.mainTexture = texture;
                imageObjects.Add(imageObject);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < textures.Count; i++)
        {
            if (textures[i] != null && imageObjects[i] != null)
            {
                imageObjects[i].SetActive(true);
            }
        }
    }
}