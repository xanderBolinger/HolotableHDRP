using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Transactions;
using System.Linq;

public class TwoWayTable
{
    private Dictionary<string, Dictionary<string, string>> tableData;

    public TwoWayTable(TextAsset csvFile)
    {

        ParseCSV(csvFile.text);
    }

    private void ParseCSV(string csvText)
    {
        tableData = new Dictionary<string, Dictionary<string, string>>();

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

            Dictionary<string, string> rowData = new Dictionary<string, string>();

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
        if (tableData.ContainsKey(y)) {
            var li = tableData[y].Keys.ToList().OrderBy(i => int.Parse(i));
            foreach (string item in li)
                if (x <= int.Parse(item))
                    return tableData[y][item];
        }
            
        throw new System.Exception("Value not found in table for x: " + x + ", y: " + y);
    }

    public string GetValue(int x, int y)
    {
        var li = tableData.Keys.ToList().OrderBy(i => int.Parse(i));
        foreach (string row in li)
            if (y <= int.Parse(row)) {
                var li2 = tableData[row].Keys.ToList().OrderBy(i => int.Parse(i));
                foreach (string item in li2)
                    if (x <= int.Parse(item))
                        return tableData[row][item];
            }
                

        throw new System.Exception("Value not found in table for x: " + x + ", y: " + y);
    }

    public string GetValue(string x, int y)
    {
        var li = tableData.Keys.ToList().OrderBy(i => int.Parse(i));
        foreach (string row in li)
            if (y <= int.Parse(row))
                return tableData[row][x];

        throw new System.Exception("Value not found in table for x: " + x + ", y: " + y);
    }

}
