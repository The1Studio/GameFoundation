using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GoogleSheetsToUnity;
using UnityEngine;
using UnityEngine.Events;

public static class SyncGoogleSheetData
{
    public static  void GetCsvFromWorkSheet(string spreadSheetID, string workSheetName, UnityAction<string> UpdateCsv = null)
    {
        SpreadsheetManager.Read(new GSTU_Search(spreadSheetID, workSheetName), ss => { UpdateCsv?.Invoke(ExportCsvFromValue(ss)); });
    }


    public static void PushAllData(string spreadSheetID, string UploadSheetName, List<List<string>> inputRawData, UnityAction OnComplete = null)
    {
        SpreadsheetManager.Write(new GSTU_Search(spreadSheetID, UploadSheetName), new ValueRange(inputRawData), OnComplete);
    }

    private static string ExportCsvFromValue(GstuSpreadSheet ss)
    {
        var stringBuilder = new StringBuilder();

        foreach (var cells in ss.ValueRange.values)
        {
            var temp        = new List<string>();
            foreach (var cell in cells)
            {
                temp.Add(StringToCSVCell(cell));
            }
            stringBuilder.AppendLine(string.Join(",", temp));
        }

        return stringBuilder.ToString();
       
    }

    public static string StringToCSVCell(string str)
    {
        bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
        if (mustQuote)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char nextChar in str)
            {
                sb.Append(nextChar);
                if (nextChar == '"')
                    sb.Append("\"");
            }
            sb.Append("\"");
            return sb.ToString();
        }

        return str;
    }
    
    private static void SaveSheetToFile(string sheetName, string sheetData, string resourceFolderPath, string format = ".csv")
    {
        try
        {
            string filePath = Path.Combine(resourceFolderPath, sheetName + format);

            File.WriteAllText(filePath, sheetData);
            Debug.Log(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: ", ex.Message, ex.StackTrace);
            throw;
        }
    }
}