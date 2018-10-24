using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

public class IosToExcel
{
	static Dictionary<string, string> LanguageToCode = new Dictionary<string, string>()
		{
			{ "English", "en" },
			{ "Japanese", "ja" },
			{ "German", "de" },
			{ "French (France)", "fr" },
			{ "Spanish (Spain)", "es" },
			{ "Simplified Chinese", "zh-Hans" },
			{ "Italian", "it" },
			{ "Dutch", "nl" },
			{ "Portuguese (Portugal)", "pt" },
			{ "Swedish", "sv" },
			{ "Finnish", "fi" },
			{ "Norwegian", "nb-NO" },
			{ "Danish", "da" },
			{ "Estonian", "et" },
			{ "Latvian", "lv" },
			{ "Lithuanian", "lt" },
			{ "French (Canada)", "fr-CA" },
			{ "Portuguese (Brazil)", "pt-BR" },
			{ "Spanish (Mexico)", "es-MX" },
			{ "Turkish", "tr-TR" },
			{ "Greek", "el" },
			{ "Traditional Chinese", "zh-Hant" },
			{ "Thai", "th-TH" },
			{ "Indonesian (Bahasa Indonesia)", "id" },
			{ "Russian", "ru-RU" },
			{ "Polish", "pl" },
			{ "Hebrew", "he" },
			{ "Hungarian", "hu" },
			{ "Slovakian", "sk" },
			{ "Czech", "cs" },
			{ "Arabic", "ar" }
		};

	public IosToExcel()
	{
	}

	public int IdColumnIndex { get; set; }
	public int EnglishColumnIndex { get; set; }
	public string FileName { get; set; }
	public string FolderName { get; set; }
	public List<IosToExcelReadParam> ReadParams { get; } = new List<IosToExcelReadParam>();

	IosToExcelInfo iosToExcelInfo;

	public enum IosToExcelParamType
	{
		Xliff = 0,
		StringToString = 1,
		StringToVar = 2
	}

	public class IosToExcelReadParam
	{
		public IosToExcelReadParam(string mask, IosToExcelParamType paramType)
		{
			Mask = mask;
			ParamType = paramType;
		}

		public string Mask { get; private set; }
		public IosToExcelParamType ParamType { get; private set; }
	}

	public class IosToExcelInfo
	{
		public IosToExcelInfo()
		{

		}

		public Dictionary<string, string> stringsToLookFor = new Dictionary<string, string>();
		public Dictionary<string, int> stringsFromLanguage = new Dictionary<string, int>();
		public Dictionary<string, string> stringsMatching = new Dictionary<string, string>();
		public Exception Error;
		public string OutputFileName;
	}

	public IosToExcelInfo Export(ProgressBar bar)
	{
		iosToExcelInfo = new IosToExcelInfo();
		if (readStringsFromExcel())
		{
			string code = getCodeWithFile(ReadParams.First().Mask);
			if (String.IsNullOrEmpty(code))
			{
				iosToExcelInfo.Error = new Exception("No .xliff file found in given path.");
			}
			else
			{
				Dictionary<string, string> stringsFromXliff = readStringsForLanguageCode(code, true);
				if (stringsFromXliff != null)
				{
					matchStrings(stringsFromXliff, bar);
					writeMatchingStringsToExcel();
				}
			}
		}
		return iosToExcelInfo;
	}

	private string getCodeWithFile(string mask)
	{
		foreach(KeyValuePair<string, string> item in LanguageToCode)
		{
			string fileName = Path.Combine(FolderName, String.Format(mask, item.Value));
			if(File.Exists(fileName))
			{
				return item.Value;
			}
		}
		return String.Empty;
	}

	private void matchStrings(Dictionary<string, string> stringsFromXliff, ProgressBar bar)
	{
		try
		{
			bar.Maximum = stringsFromXliff.Count * iosToExcelInfo.stringsToLookFor.Count;
			bar.Value = 0;
			foreach (KeyValuePair<string, string> xliff in stringsFromXliff)
			{
				foreach (KeyValuePair<string, string> lookFor in iosToExcelInfo.stringsToLookFor)
				{
					bar.Increment(1);
					if (xliff.Value == lookFor.Value)
					{
						if (iosToExcelInfo.stringsMatching.ContainsKey(lookFor.Key))
						{
							//throw new Exception("Duplicate key detected \"" + lookFor.Key + "\"");
						}
						else
						{
							iosToExcelInfo.stringsMatching.Add(lookFor.Key, xliff.Key);
						}
					}
				}
			}
		} catch(Exception e)
		{
			iosToExcelInfo.Error = new Exception("Matching strings", e);
		}
	}

	private void writeMatchingStringsToExcel()
	{
		try
		{
			using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(FileName)))
			{
				var workSheet = xlPackage.Workbook.Worksheets.First();
				var totalRows = workSheet.Dimension.End.Row;
				var totalCols = workSheet.Dimension.End.Column;

				// assumptions:
				// 1. first row contain titles
				// 2. first column contains ids, no id will be ignored
				// 3. second column contains english strings, so we start on the third column
				for (int col = EnglishColumnIndex + 1; col <= totalCols; col++)
				{
					var language = workSheet.Cells[1, col].Text;
					if (LanguageToCode.ContainsKey(language))
					{
						var languageCode = LanguageToCode[language];
						Dictionary<string, string> stringsFromXliff = readStringsForLanguageCode(languageCode, false);
						if (stringsFromXliff != null)
						{
							for (int row = 2; row <= totalRows; row++)
							{
								var id = workSheet.Cells[row, IdColumnIndex].Text;
								if (!String.IsNullOrEmpty(id) && 
									iosToExcelInfo.stringsMatching.ContainsKey(id) &&
									stringsFromXliff.ContainsKey(iosToExcelInfo.stringsMatching[id]))
								{
									workSheet.Cells[row, col].Value = stringsFromXliff[iosToExcelInfo.stringsMatching[id]];
								}
							}
						}
					}
				}

				FileInfo output = createNewFile();
				xlPackage.SaveAs(output);
				iosToExcelInfo.OutputFileName = output.Name;
			}
		}
		catch (Exception e)
		{
			iosToExcelInfo.Error = new Exception("Writing matching strings to excel", e);
		}
	}

	private FileInfo createNewFile()
	{
		int count = 0;
		FileInfo newFile;
		do
		{
			count++;
			string newFileName = FileName.Replace(".xlsx", "_" + count + ".xlsx");
			newFile = new FileInfo(newFileName);
		} while (newFile.Exists);
		return newFile;
	}

	private bool readStringsFromExcel()
	{
		try
		{
			iosToExcelInfo.stringsToLookFor.Clear();
			using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(FileName)))
			{
				var workSheet = xlPackage.Workbook.Worksheets.First();
				var totalRows = workSheet.Dimension.End.Row;
				var totalCols = workSheet.Dimension.End.Column;

				// assumptions:
				// 1. first row contain titles
				// 2. first column contains ids, no id will be ignored
				// 3. second column contains english strings
				for (int row = 2; row <= totalRows; row++)
				{
					var id = workSheet.Cells[row, IdColumnIndex].Text;
					if (!String.IsNullOrEmpty(id))
					{
						var value = workSheet.Cells[row, EnglishColumnIndex].Text.Trim(new char[] { ' ', (char)160 }).Replace("\'", "\\\'").Replace("\\\\", "\\");
						if (iosToExcelInfo.stringsToLookFor.ContainsKey(id))
						{
							throw new Exception("Duplicate key detected \"" + id + "\".\nPlease review your Excel file.");
						}
						else
						{
							iosToExcelInfo.stringsToLookFor.Add(id, value);
						}
					}
				}
			}
		}
		catch (Exception e)
		{
			iosToExcelInfo.Error = new Exception("Reading strings from excel", e);
			return false;
		}
		return true;
	}

	private Dictionary<string, string> readStringsForLanguageCode(string code, bool readSource)
	{
		Dictionary<string, string> stringsFromLanguage = new Dictionary<string, string>();
		foreach (IosToExcelReadParam readParam in ReadParams)
		{
			switch (readParam.ParamType)
			{
				case IosToExcelParamType.Xliff:
					readStringsFromXliff(readParam.Mask, code, readSource, stringsFromLanguage);
					break;
				case IosToExcelParamType.StringToString:
					readStringsFromStrings(readParam.Mask, code, readSource, stringsFromLanguage, "^\"(?<id>.*)\"\\s*=\\s*\"(?<val>.*)\";$");
					break;
				case IosToExcelParamType.StringToVar:
					readStringsFromStrings(readParam.Mask, code, readSource, stringsFromLanguage, "^(?<id>\\S*)\\s*=\\s*\"(?<val>.*)\";$");
					break;
			}
			if(iosToExcelInfo.Error != null)
			{
				break;
			}
		}
		return stringsFromLanguage;
	}

	private void readStringsFromStrings(string mask, string code, bool readSource, Dictionary<string, string> stringsFromLanguage, string pattern)
	{
		string fileName = Path.Combine(FolderName, String.Format(mask, code));
		try
		{
			string line;
			StreamReader file = new StreamReader(fileName);
			Regex regex = new Regex(pattern);
			while ((line = file.ReadLine()) != null)
			{
				Match match = regex.Match(line);
				if (match.Success && match.Groups.Count == 3)
				{
					string source = match.Groups["id"].Value.Trim(new char[] { ' ' });
					string target = match.Groups["val"].Value.Trim(new char[] { ' ' });
					string text = String.Empty;

					if (readSource)
					{
						text = source;
					}
					else if (target != source)
					{
						text = target;
					}

					if (stringsFromLanguage.ContainsKey(source))
					{
						if (stringsFromLanguage[source] != text)
						{
							// for now ignore duplicate exception
							throw new Exception("Duplicate key detected \"" + source + "\"");
						}
					}
					else
					{
						stringsFromLanguage.Add(source, text);
					}
				}
			}
			file.Close();
			updateStringsFromLanguage(code, readSource, stringsFromLanguage.Count);
		}
		catch(Exception ex)
		{
			iosToExcelInfo.Error = new Exception("Reading strings from \"" + fileName + "\"", ex);
		}
	}

	private void readStringsFromXliff(string mask, string code, bool readSource, Dictionary<string, string> stringsFromLanguage)
	{	
		string fileName = Path.Combine(FolderName, String.Format(mask, code));
		try
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(fileName);

			XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
			manager.AddNamespace("ns", "urn:oasis:names:tc:xliff:document:1.2");

			XmlNodeList nodes = doc.GetElementsByTagName("trans-unit");
			foreach (XmlNode node in nodes)
			{
				string id = node.Attributes["id"].Value;
				string target = String.Empty;
				string source = String.Empty;
				string text = String.Empty;

				foreach (XmlNode child in node.ChildNodes)
				{
					if (child.Name == "target")
					{
						target = child.InnerText;
					}
					else if (child.Name == "source")
					{
						source = child.InnerText;
					}
				}

				if (readSource)
				{
					text = source;
				}
				else if(target != source)
				{
					text = target;
				}

				text = text.Trim(new char[] { ' ', (char)160 }).Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\"", "\\\"").Replace("'", "\\'");

				if (!String.IsNullOrEmpty(text))
				{
					if (id == "CFBundleDisplayName" || id == "CFBundleName")
					{
						// just ignore these guys
					}
					else if (stringsFromLanguage.ContainsKey(id))
					{
						if (stringsFromLanguage[id] != text)
						{
							// for now ignore duplicate exception
							// throw new Exception("Duplicate key detected \"" + id + "\"");
						}
					}
					else
					{
						stringsFromLanguage.Add(id, text);
					}
				}
			}
			updateStringsFromLanguage(code, readSource, stringsFromLanguage.Count);
		}
		catch (Exception ex)
		{
			iosToExcelInfo.Error = new Exception("Reading strings from \"" + fileName+ "\"", ex);
		}
	}

	private void updateStringsFromLanguage(string code, bool readSource, int count)
	{
		string key = code;
		if (readSource)
		{
			key = "source(" + code + ")";
		}
		if (iosToExcelInfo.stringsFromLanguage.ContainsKey(key))
		{
			iosToExcelInfo.stringsFromLanguage[key] += count;
		}
		else
		{
			iosToExcelInfo.stringsFromLanguage.Add(key, count);
		}
	}
}
