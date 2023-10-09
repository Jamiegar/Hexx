using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_InputWindow
{

    public UI_InputWindow(TMP_InputField inputField, string validChars)
    {
        inputField.onValidateInput = (string text, int charIndex, char addedChar) =>
        {
            return ValidateChar(validChars, addedChar);
        };

        inputField.characterLimit = 4;
    }


    private char ValidateChar(string validCharacters, char addedChar)
    {
        if (validCharacters.IndexOf(addedChar) != -1)
        {
            //Returns valid char
            return addedChar;
        }
        else
        {
            //char is invalid
            return '\0';
        }

    }
}
