using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

// =====================================================================
//  ROCK - PAPER - SCISSORS  |  Lab Exercise
// =====================================================================
//  Scene setup:
//  [ ] Images: ImgYou, ImgCom      Texts (Legacy): TxtYou, TxtCom, TxtResult
//  [ ] Buttons: 1_Rock, 2_Paper, 3_Scissors
//  [ ] Assets/Resources/: img_1 (rock), img_2 (paper), img_3 (scissors) as Sprites
//  [ ] Each button OnClick -> GameManager.OnButtonClick -> drag the button itself in
//
//  Number code:  1 = Rock   2 = Paper   3 = Scissors
// =====================================================================

public class GameManagerGuide : MonoBehaviour
{
    // STEP 1 - Variables
    // TODO: Declare 2 Images (you, com), 3 Texts (your score, com score, result), 2 int counters.
    // HINT: Image imgYou;   Text txtYou;   int cntYou = 0;'
    Image imgYou;
    Image imgCom;

    TMP_Text Txt1;
    TMP_Text Txt2;
    TMP_Text txtResult;

    int cntYou = 0;
    int cntCom = 0;


    // STEP 2 - Find UI objects
    // TODO: Find each UI object by name, then set the result text to "Select the button below".
    // HINT: imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
    private void InitGame()
    {
        imgYou = GameObject.Find("ImgYou").GetComponent<Image>();
        imgCom = GameObject.Find("ImgCom").GetComponent<Image>();

        Txt1 = GameObject.Find("ImgScore_You").transform.Find("Txt").GetComponent<TMP_Text>();
        Txt2 = GameObject.Find("ImgScore_Com").transform.Find("Txt").GetComponent<TMP_Text>();
        txtResult = GameObject.Find("TxtResult").GetComponent<TMP_Text>();

        txtResult.text = "Select the option below";
        Txt1.text = cntYou.ToString();
        Txt2.text = cntCom.ToString();
    }


    // STEP 3 - Start the game
    // TODO: Call InitGame() when the game starts.
    // HINT: Start() runs once, before the first frame.
    void Start()
    {
        InitGame();
    }


    // STEP 4 - Quit with Escape
    // TODO: Quit the game when the Escape key is pressed.
    // HINT: Input.GetKeyDown(KeyCode.Escape) and Application.Quit()
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    // STEP 5 - Read the clicked button
    // TODO: Turn the first character of the button name into an int and pass it to CheckResult().
    // HINT: int.Parse(buttonObject.name.Substring(0, 1))
    //
    // EXPLAIN:
    //   All 3 buttons call this SAME function. Each button sends ITSELF
    //   (the object you dragged into the OnClick slot), so we use its NAME
    //   to know which one was clicked.
    //
    //   Example: player clicks the "2_Paper" button
    //     buttonObject.name          -> "2_Paper"   (string)
    //     .Substring(0, 1)           -> "2"         (1 character, starting at position 0)
    //     int.Parse("2")             -> 2           (now a number we can calculate with)
    //     CheckResult(2)             -> send it to Step 6
    //
    //   That is why button names MUST start with 1, 2 or 3.
    //   A button named "Paper" would crash: int.Parse("P") is not a number.
    public void OnButtonClick(GameObject buttonObject)
    {
        Debug.Log(buttonObject.name + " clicked.");

        int yourResult = int.Parse(buttonObject.name.Substring(0, 1));

        CheckResult(yourResult);
    }


    // STEP 6 - Decide who wins
    // TODO: Pick a random computer number 1-3, compare to yourResult, update score + result text, then call SetResult().
    // HINT: Random.Range(1, 4); try k = yourResult - comResult -> you win when k is 1 or -2.
    //
    // EXPLAIN:
    //   a) Computer picks a number
    //      Random.Range(1, 4) with int numbers does NOT include the max,
    //      so it gives 1, 2 or 3. (Random.Range(1, 3) would never pick Scissors!)
    //
    //   b) Find the winner with one subtraction: k = yourResult - comResult
    //
    //        You          Com          k      Result
    //        2 Paper      1 Rock       1      You win
    //        3 Scissors   2 Paper      1      You win
    //        1 Rock       3 Scissors  -2      You win
    //        same         same         0      Draw
    //        anything else          -1 or 2   Computer wins
    //
    //      Each number beats the one just below it, and Rock (1) wraps
    //      around to beat Scissors (3). So:
    //        if k == 0               -> Draw
    //        else if k == 1 or -2    -> You win     (add 1 to your counter)
    //        else                    -> Computer win (add 1 to computer counter)
    //
    //   c) Put the message in the result Text, e.g. txtResult.text = "You win.";
    //
    //   d) Finally call SetResult(yourResult, comResult) to send BOTH numbers to Step 7.
    void CheckResult(int yourResult)
    {
        int comResult = Random.Range(1, 4);
        int k = yourResult - comResult;

        if (k == 0)
        {
            txtResult.text = "Draw!";
        }
        else if (k == 1 || k == -2)
        {
            txtResult.text = "You win!";
            cntYou++;
        }
        else
        {
            txtResult.text = "Computer wins!";
            cntCom++;
        }

        SetResult(yourResult, comResult);
    }


    // STEP 7 - Update the UI
    // TODO: Load both sprites, flip the computer image, and show both scores.
    // HINT: Resources.Load("img_" + you, typeof(Sprite)) as Sprite;  localScale = new Vector3(-1, 1, 1);  .ToString()
    //
    // EXPLAIN:
    //   a) Change the pictures
    //      "img_" + you              -> "img_2"  (file name, no .png)
    //      Resources.Load(...)       -> looks ONLY inside the Assets/Resources folder
    //      typeof(Sprite) / as Sprite -> load it as a Sprite (the type an Image can show)
    //      imgYou.sprite = ...       -> put that picture on the Image
    //      Do the same for imgCom with the "com" number.
    //      Picture blank? -> file not in Resources, or Texture Type is not "Sprite (2D and UI)".
    //
    //   b) Flip the computer's hand
    //      imgCom.transform.localScale = new Vector3(-1, 1, 1);
    //      x = -1 mirrors the image left <-> right, so both hands face each other.
    //      (We reuse the same 3 pictures for both sides.)
    //
    //   c) Show the scores
    //      .text only accepts a string, but our counters are int,
    //      so convert first: txtYou.text = cntYou.ToString();  (same for computer)
    void SetResult(int you, int com)
    {
        imgYou.sprite = Resources.Load("img_" + you, typeof(Sprite)) as Sprite;

        imgCom.sprite = Resources.Load("img_" + com, typeof(Sprite)) as Sprite;

        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        Txt1.text = cntYou.ToString();
        Txt2.text = cntCom.ToString();
    }


    // STEP 8 - Test
    // TODO: Play 20 rounds and check images, result text, and scores are all correct.
    // HINT: A Draw should not change either score.
}
