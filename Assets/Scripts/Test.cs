using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Test : MonoBehaviour
{
    public string[] urls;
    public float delay = 1f;

    IEnumerator Start()
    {
        List<UnityWebRequest> requests = new List<UnityWebRequest>();

        foreach (string url in urls)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            requests.Add(www);
            www.SendWebRequest();
        }

        while (requests.Count > 0)
        {
            for (int i = 0; i < requests.Count; i++)
            {
                UnityWebRequest www = requests[i];

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(www.error);
                    requests.RemoveAt(i);
                    i--;
                }
                else if (www.isDone)
                {
                    Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    GameObject imageObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    imageObject.GetComponent<Renderer>().material.mainTexture = texture;

                    requests.RemoveAt(i);
                    i--;

                    yield return new WaitForSeconds(delay);
                }
            }

            yield return null;
        }
    }
}