using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class JRPG_Dialog : MonoBehaviour
{
    //Just a test string.
    private string test_text = "This is a test text, where it will show how the text will work and its behavior, good luck! :D!." +
                               "\nOh and feel free to contribute and make this code better.";

    //A array containing all lines from this dialogue.
    private string[] lines;

    //If the dialog box is waiting a input to continue showing other lines.
    private bool isWaitingInput = true;

    //The time count to show a char.
    private float currentCharDelay = 0;

    //Wich line the dialog will show now, used on update.
    private char[] currentLine;

    //Number used to iterate current line.
    private int charNumber;

    //How much lines are in the dialog box now.
    private int lineBoxNumber = 0;

    //Current line number in lines array.
    private int lineNumber = 0;

    //The char limit in a line.
    public int lineCharThreshold = 50;

    //How many lines there is in the dialog box.
    public int lineLimit = 3;

    //The speed that chars will be appearing in the dialog box. (The time between chars)
    public float charSpeed = 50;

    //Text that we will show our dialog.
    public Text dialog_text;

    public void Start()
    {
    }

	
	void Update () 
    {
        if (!isWaitingInput)
        {
            //add time to currentCharDelay to show char.
            currentCharDelay += Time.deltaTime * charSpeed;

            if (currentCharDelay >= 1)
            {
                currentCharDelay = 0;
                if (charNumber < currentLine.Length)
                {
                    dialog_text.text += currentLine[charNumber];
                    charNumber += 1;
                }
                else if (lineBoxNumber < lineLimit-1 && lineNumber < lines.Length-1)
                {
                    charNumber = 0;
                    dialog_text.text += "\n";
                    lineBoxNumber += 1;
                    lineNumber += 1;
                    currentLine = lines[lineNumber].ToCharArray();
                }
                else
                {
                    isWaitingInput = true;
                }

            }
        }
	}

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void CloseDialog()
    {
        ClearDialog();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Clears JRPG_Dialog class.
    /// </summary>
    private void ClearDialog()
    {
        lines = null;
        isWaitingInput = true;
        currentCharDelay = 0;
        currentLine = null;
        charNumber = 0;
        lineNumber = 0;
        lineBoxNumber = 0;
        dialog_text.text = "";
    }


    /// <summary>
    /// Starts the dialog, use text variable to load the right text.
    /// </summary>
    public void StartDialog(string text)
    {
        gameObject.SetActive(true);
        isWaitingInput = false;

        //The test_text is just a placeholder. Use text instead when using on your project.
        LoadText(test_text);
        currentLine = lines[lineNumber].ToCharArray();
    }

    /// <summary>
    /// Handles the user input.
    /// </summary>
    public void InputHandler()
    {
        if (!isWaitingInput)
        {
            return;
        }

        lineNumber += 1;
        //If lineNumber is higher than the lenght of lines, it means that our dialog endend.
        if (lineNumber >= lines.Length)
        {
            CloseDialog();
        }
        else
        {
            currentLine = lines[lineNumber].ToCharArray();
            dialog_text.text = "";
            charNumber = 0;
            lineBoxNumber = 0;
            isWaitingInput = false;
        }
    }

    /// <summary>
    /// Separate the text in lines, every line number of char is equal or less than lineCharThreshold.
    /// </summary>
    /// <param name="fullText">Full text.</param>
    public void LoadText(string fullText)
    {
        fullText = fullText.Replace("\n", " \n ");
        string[] allLines = fullText.Split(new char[] {' ','\t'},StringSplitOptions.None);

        int charCount = 0;
        int lineCount = 0;

        string[] auxLines = new string[allLines.Length/lineLimit];


        //Divide our words in lines.
        for (int i = 0; i < allLines.Length; i++)
        {
            //Check for new lines.
            if (allLines[i].Equals("\n"))
            {
                lineCount += 1;
                charCount = 0;
                continue;
            }

            if (charCount + allLines[i].ToCharArray().Length <= lineCharThreshold)
            {
                auxLines[lineCount] += allLines[i]+" ";
                charCount += allLines[i].ToCharArray().Length+1;
            }
            else
            {
                lineCount += 1;
                charCount = 0;
                i--;
                continue;
            }
        }
            
        lines = new string[lineCount+1];
        for (int i = 0; i < lineCount+1; i++)
        {
            lines[i] = auxLines[i];
            Debug.Log("Lines: " + lines[i]);
        }
    }
}