using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Localizationeer
{
	public class ExcelToAndroid
	{
		static Dictionary<string, string> LanguageToCode = new Dictionary<string, string>()
		{
			{ "English", String.Empty },
			{ "Japanese", "ja" },
			{ "German", "de" },
			{ "French (France)", "fr" },
			{ "Spanish (Spain)", "es" },
			{ "Simplified Chinese", "zh" },
			{ "Italian", "it" },
			{ "Dutch", "nl" },
			{ "Portuguese (Portugal)", "pt" },
			{ "Swedish", "sv" },
			{ "Finnish", "fi" },
			{ "Norwegian", "nb-rNO" },
			{ "Danish", "da" },
			{ "Estonian", "et" },
			{ "Latvian", "lv" },
			{ "Lithuanian", "lt" },
			{ "French (Canada)", "fr-rCA" },
			{ "Portuguese (Brazil)", "pt-rBR" },
			{ "Spanish (Mexico)", "es-rMX" },
			{ "Turkish", "tr-rTR" },
			{ "Greek", "el" },
			{ "Traditional Chinese", "zh-rHK,zh-rTW" },
			{ "Thai", "th-rTH" },
			{ "Indonesian (Bahasa Indonesia)", "id,in" },
			{ "Russian", "ru-rRU" },
			{ "Polish", "pl" },
			{ "Hebrew", "he,iw" },
			{ "Hungarian", "hu" },
			{ "Slovakian", "sk" },
			{ "Czech", "cs" },
			{ "Arabic", "ar" }
		};

		public ExcelToAndroid()
		{
		}

		public int IdColumnIndex { get; set; }
		public int EnglishColumnIndex { get; set; }
		public string FileName { get; set; }
		public string FolderName { get; set; }


		public class ExcelToAndroidInfo
		{
			public ExcelToAndroidInfo()
			{

			}

			public Dictionary<string, string> summary = new Dictionary<string, string>();
			public Exception Error;
		}

		public ExcelToAndroidInfo ReadExcelAndApplyNewValues()
		{
			ExcelToAndroidInfo excelToAndroidInfo = new ExcelToAndroidInfo();
			try
			{
				using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(FileName)))
				{
					var workSheet = xlPackage.Workbook.Worksheets.First();
					var totalRows = workSheet.Dimension.End.Row;
					var totalCols = workSheet.Dimension.End.Column;

					for (int col = EnglishColumnIndex; col <= totalCols; col++)
					{
						var language = workSheet.Cells[1, col].Text;
						if (LanguageToCode.ContainsKey(language))
						{
							var languageCode = LanguageToCode[language];

							Dictionary<string, string> values = new Dictionary<string, string>();

							for (int row = 2; row <= totalRows; row++)
							{
								var id = workSheet.Cells[row, IdColumnIndex].Text;
								if (id != String.Empty)
								{
									var value = workSheet.Cells[row, col].Text.Trim(new char[] { ' ', (char)160 }).Replace("\'", "\\\'").Replace("\\\\", "\\");
									if (values.ContainsKey(id))
									{
										throw new Exception("Duplicate key detected \"" + id + "\".\nPlease review your Excel file.");
									}
									values.Add(id, value);
								}
							}

							if (values.Count > 0)
							{
								string[] languageCodes = languageCode.Split(',');
								foreach (string code in languageCodes)
								{
									string fileName = Path.Combine(FolderName, "values" + (code == String.Empty ? String.Empty : "-" + code) + "\\strings.xml");

									string languageAndCode = language + (code == String.Empty ? String.Empty : " (" + code + ")");
									string output = SetValuesInXml(values, fileName);

									excelToAndroidInfo.summary.Add(languageAndCode, output);
								}
							}
						}
					}
				}
			}
			catch (IOException e)
			{
				excelToAndroidInfo.Error = new Exception("Reading excel and applying new values", e);
			}
			return excelToAndroidInfo;
		}

		private string SetValuesInXml(Dictionary<string, string> values, string fileName)
		{
			int count = 0;

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.PreserveWhitespace = true;
				doc.Load(fileName);

				XmlNode parent = doc.SelectSingleNode("/resources");
				foreach (KeyValuePair<string, string> value in values)
				{
					if (!String.IsNullOrEmpty(value.Value))
					{
						XmlNodeList nodes = doc.SelectNodes("/resources/string[@name='" + value.Key + "']");

						if (nodes.Count == 0)
						{
							XmlNode indent = doc.CreateTextNode("    ");
							parent.AppendChild(indent);
							XmlAttribute attr = doc.CreateAttribute("name");
							attr.Value = value.Key;
							XmlNode node = doc.CreateNode(XmlNodeType.Element, "string", null);
							node.Attributes.Append(attr);
							node.InnerXml = value.Value;
							parent.AppendChild(node);
							XmlNode lineBreak = doc.CreateTextNode("\n");
							parent.AppendChild(lineBreak);
							count++;
						}
						else
						{
							foreach (XmlNode node in nodes)
							{
								if (node.InnerXml != value.Value)
								{
									node.InnerXml = value.Value;
									count++;
								}
							}
						}
					}
				}

				doc.Save(fileName);
			}
			catch (Exception e)
			{
				return "Aborted: " + e.Message;
			}

			return count.ToString();
		}
	}
}