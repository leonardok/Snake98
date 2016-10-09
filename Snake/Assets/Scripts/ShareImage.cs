using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ShareImage : MonoBehaviour
{
    public GameObject[] disableElementsBefore;
    public GameObject[] disableElementsAfter;
    public int sharedImageWidth = 600;

    private bool isProcessing = false;

    private string gameLink = "Download the game on play store at https://play.google.com/store/apps/details?id=me.leok.retrosnake";
    private string subject = "Snake 98 Game";
    private string imageName = "Snake98Screenshot"; // without the extension, for iinstance, MyPic 


    private void disableElementsBeforePrint()
    {
        foreach (GameObject i in disableElementsBefore)
        {
            i.SetActive(false);
        }
    }

    private void disableElementsAfterPrint()
    {
        foreach (GameObject i in disableElementsAfter)
        {
            i.SetActive(false);
        }
    }

    public void shareImage()
    {
        if (!isProcessing)
            StartCoroutine(ShareScreenshot());

    }

    private IEnumerator ShareScreenshot(){
        isProcessing = true;

        // just disable the elements that are not to show in print
        disableElementsBeforePrint();

        yield return new WaitForEndOfFrame();

        // create the texture
        //Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);

        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        screenTexture.Apply();

        Debug.Log("ShareScreenshot");

        byte[] dataToSave = screenTexture.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath,
                                   imageName + System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");

        Debug.Log(path);
        File.WriteAllBytes(path, dataToSave);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            intentObject.Call<AndroidJavaObject>("setType", "image/*");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), subject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), gameLink);


            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", path);
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);


            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            // option one WITHOUT chooser:
            currentActivity.Call("startActivity", intentObject);
        }
         
        isProcessing = false;

        disableElementsAfterPrint();
    }

}