using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;

class EventCSVReader
{
    public class Record
    {
        public string[] data = new String[5];
    }
    public static void ReadFromFile(string filePath, Action<Record> process)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Record>(); // 绑定到自定义类
            foreach (var record in records)
            {
                process.Invoke(record);
            }
        }
    }
}
