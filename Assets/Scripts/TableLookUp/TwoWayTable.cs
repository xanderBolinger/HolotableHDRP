using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Transactions;

public class TwoWayTable
{
    private SortedDictionary<string, SortedDictionary<string, string>> tableData;

    public TwoWayTable(TextAsset csvFile)
    {

        ParseCSV(csvFile.text);
    }

    private void ParseCSV(string csvText)
    {
        tableData = new SortedDictionary<string, SortedDictionary<string, string>>();

        string[] lines = csvText.Split('\n');

        if (lines.Length < 2)
        {
            Debug.LogError("CSV file must have at least a header and one data row.");
            return;
        }

        string[] headers = lines[0].Trim().Split(',');
        

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(',');
            if (values.Length != headers.Length)
            {
                Debug.LogError("Row " + (i + 1) + " has a different number of columns than the header.");
                continue;
            }

            SortedDictionary<string, string> rowData = new SortedDictionary<string, string>();

            for (int j = 1; j < headers.Length; j++)
                rowData[headers[j]] = values[j];

            tableData[values[0]] = rowData;
        }
    }

    public string GetValue(string x, string y)
    {
        if (tableData.ContainsKey(y) && tableData[y].ContainsKey(x))
            return tableData[y][x];

        throw new System.Exception("Value not found in table for x: "+x+", y: "+y);
    }

    public string GetValue(int x, string y) {
        if (tableData.ContainsKey(y))
            foreach(string item in tableData[y].Keys)
                if(x <= int.Parse(item))
                    return tableData[y][item];
            
        throw new System.Exception("Value not found in table for x: " + x + ", y: " + y);
    }

    public string GetValue(int x, int y)
    {
        foreach(string row in tableData.Keys)
            if(y<= int.Parse(row))
                foreach (string item in tableData[row].Keys)
                    if (x <= int.Parse(item))
                        return tableData[row][item];

        throw new System.Exception("Value not found in table for x: " + x + ", y: " + y);
    }

}
