using UnityEngine;
using TMPro;

public class CodeGenerator : MonoBehaviour
{
    public TextMeshProUGUI[] codeSlots; // Arreglo de 6 textos
    private string[] possibleLetters = { "A", "B", "Z", "X" };
    private string generatedCode = "";

    void Start()
    {
        //GenerateCode();
    }

    public void GenerateCode(string code)
    {
        generatedCode = "";

        for (int i = 0; i < codeSlots.Length; i++)
        {
            string randomChar = possibleLetters[Random.Range(0, possibleLetters.Length)];
            codeSlots[i].text = code[i].ToString();
            //randomChar;
            generatedCode += randomChar;
        }

        //Debug.Log("C�digo generado: " + generatedCode);
    }

    public string GetGeneratedCode()
    {
        return generatedCode;
    }
}
