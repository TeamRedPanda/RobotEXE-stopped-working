using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    void Update()
    {
        if ( Input.GetMouseButtonDown( 0 ) ) {
            SkipToNextText();
        }
    }

    [Range(0, 1)]
    public float textWaitTime = 0.03f;
    public Text textBox;

    //Store all your text in this string array
    string[] goatText = new string[]{"1. Laik's super awesome custom typewriter script", "2. You can click to skip to the next text", "3.All text is stored in a single string array", "4. Ok, now we can continue","5. End Kappa"};

    int currentlyDisplayingText = 0;
    private bool isSentenceComplete;

    void Awake()
    {
        StartCoroutine( AnimateText( goatText[currentlyDisplayingText] ) );
    }
    //This is a function for a button you press to skip to the next text
    public void SkipToNextText()
    {

        StopAllCoroutines();

        if ( isSentenceComplete == false ) {
            textBox.text = goatText[currentlyDisplayingText];
            isSentenceComplete = true;
            return;
        } else {
            currentlyDisplayingText++;
        }
        //If we've reached the end of the array, do anything you want. I just restart the example text
        if ( currentlyDisplayingText >= goatText.Length ) {
            currentlyDisplayingText = 0;
        }
        StartCoroutine( AnimateText( goatText[currentlyDisplayingText] ) );
    }
    //Note that the speed you want the typewriter effect to be going at is the yield waitforseconds (in my case it's 1 letter for every      0.03 seconds, replace this with a public float if you want to experiment with speed in from the editor)
    IEnumerator AnimateText(string displayText)
    {
        isSentenceComplete = false;

        for ( int i = 0 ; i <= ( displayText.Length ) ; i++ ) {
            textBox.text = displayText.Substring( 0 , i );
            yield return new WaitForSeconds( textWaitTime );
        }

        isSentenceComplete = true;
    }
}
