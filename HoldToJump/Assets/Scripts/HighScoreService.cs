using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HighScoreService : MonoBehaviour
{
    public static HighScoreService instance;

    private string secretKey = "123456789";
    string highscoreURL = "clawindiegames.square7.ch/display.php";
    string addScoreURL = "clawindiegames.square7.ch/addscore.php?"; //be sure to add a ? to your url

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator<WWW> FillListWithHighScores (Action<WWW> scoresWWW)
    {
        WWW hs_get = new WWW("http://" + highscoreURL);
        yield return hs_get;
        
        if (hs_get.error != null)
        {
            Debug.LogError("There was an error getting the high score: " + hs_get.error);
        }
        // Callback function
        scoresWWW(hs_get);               
    }
        
    public void UpdatePlayerHighScore(string playerId, string playerName, int score)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            StartCoroutine(PostScores(playerId, playerName, score));
        }
    }

    private IEnumerator PostScores(string uniqueID, string name, int score)
    {
        //updateOnlineHighscoreData ();

        //This connects to a server side php script that will add the name and score to a MySQL DB.
        // Supply it with a string representing the players name and the players score.
        string hash = Md5Sum(name + score + secretKey);
        //string post_url = addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        string post_url = addScoreURL + "uniqueID=" + uniqueID + "&name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;
        //Debug.Log ("post url " + post_url);
        // Post the URL to the site and create a download object to get the result.

        WWW hs_post = new WWW("http://" + post_url);
        yield return hs_post; // Wait until the download is done

        if (hs_post.error != null)
        {
            Debug.LogError("There was an error posting the high score: " + hs_post.error);
        }
    }

    private string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";

        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }

        return hashString.PadLeft(32, '0');
    }
}
