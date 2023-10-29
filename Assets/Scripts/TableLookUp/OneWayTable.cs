using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OneWayTable
{
    private Dictionary<string, string> tableData;

    public OneWayTable(TextAsset csvFile)
    {
        ParseCSV(csvFile.text);
    }

    private void ParseCSV(string csvText)
    {
        tableData = new Dictionary<string, string>();

        string[] lines = csvText.Split('\n');

        if (lines.Length < 2)
        {
            Debug.LogError("CSV file must have at least a header and one data row.");
            return;
        }

        string[] headers = lines[0].Trim().Split(',');

        if (headers.Length != 2)
        {
            Debug.LogError("The CSV file should have exactly two columns (key, value).");
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Trim().Split(',');

            if (values.Length != 2)
            {
                Debug.LogWarning("Skipping row " + (i + 1) + " due to incorrect format.");
                continue;
            }

            string key = values[0];
            string value = values[1];
            tableData[key] = value;
        }
    }

    public string GetValue(string key)
    {
        if (tableData.ContainsKey(key))
        {
            return tableData[key];
        }

        throw new System.Exception("Value not found in table for value: "+key);
    }

    public string GetValue(int key)
    {
        var li = tableData.Keys.ToList().OrderBy(i => int.Parse(i));
        foreach (var k in li) 
            if(key <= int.Parse(k))
                return tableData[k];
        

        throw new System.Exception("Value not found in table for value: " + key);
    }

}
