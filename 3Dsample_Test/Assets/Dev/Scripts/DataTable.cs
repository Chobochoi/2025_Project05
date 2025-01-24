using UnityEngine;

public class DataTable : MonoBehaviour
{
    public TextAsset dataText;
    string[,] dataMatrix;
    int lineSize, rowSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 엔터 단위와 탭으로 나눠서 배열의 크기 조정
        string currentText = dataText.text.Substring(0, dataText.text.Length - 1);
        string[] lines = currentText.Split('\n');
        lineSize = lines.Length;
        rowSize = lines[0].Split('\t').Length;
        dataMatrix = new string[lineSize, rowSize];

        for (int i = 0; i < lineSize; i++)
        {
            string[] row = lines[i].Split("\t");
            for (int j = 0; j < rowSize; j++)
            {
                dataMatrix[i, j] = row[j];
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
