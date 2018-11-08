using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Localizationeer
{
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

		int threshold = 100;
		public int Threshold
		{
			get
			{
				return threshold;
			}
			set
			{
				threshold = Math.Min(100, Math.Max(0, value));
			}
		}

		IosToExcelInfo iosToExcelInfo;

		public class IosToExcelInfo
		{
			public IosToExcelInfo()
			{

			}

			public Dictionary<string, string> stringsToLookFor = new Dictionary<string, string>();
			public Dictionary<string, int> stringsFromLanguage = new Dictionary<string, int>();
			public Dictionary<string, string> stringsMatching = new Dictionary<string, string>();
			public Dictionary<string, int> howClose = new Dictionary<string, int>();
			public Dictionary<string, string> howCloseMatching = new Dictionary<string, string>();
			public Exception Error;
			public string OutputFileName;
		}

		public IosToExcelInfo Export(ProgressBar bar)
		{
			iosToExcelInfo = new IosToExcelInfo();
			if (readStringsFromExcel())
			{
				string code = getCodeWithFiles();
				if (String.IsNullOrEmpty(code))
				{
					iosToExcelInfo.Error = new Exception("No files found in given path.");
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

		private string getCodeWithFiles()
		{
			foreach (KeyValuePair<string, string> item in LanguageToCode)
			{
				string[] files = FindFiles(item.Value);
				foreach (string file in files)
				{
					if (file.EndsWith("xliff"))
					{
						return item.Value;
					}
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
						int howClose = CloseEnoughComparer.Compare(xliff.Value, lookFor.Value, StringComparison.InvariantCultureIgnoreCase);
						//int howClose = LevenshteinComparer.Compare(xliff.Value, lookFor.Value, StringComparison.InvariantCultureIgnoreCase);
						bar.Increment(1);
						if (howClose >= Threshold)
						{
							if (iosToExcelInfo.stringsMatching.ContainsKey(lookFor.Key))
							{
								if (howClose > iosToExcelInfo.howClose[lookFor.Key])
								{
									iosToExcelInfo.stringsMatching[lookFor.Key] = xliff.Key;
									iosToExcelInfo.howClose[lookFor.Key] = howClose;
									iosToExcelInfo.howCloseMatching[lookFor.Key] = lookFor.Value;
								}
								//throw new Exception("Duplicate key detected \"" + lookFor.Key + "\"");
							}
							else
							{
								iosToExcelInfo.stringsMatching.Add(lookFor.Key, xliff.Key);
								iosToExcelInfo.howClose.Add(lookFor.Key, howClose);
								iosToExcelInfo.howCloseMatching.Add(lookFor.Key, xliff.Value);
							}
						}
					}
				}
			}
			catch (Exception e)
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
							var value = cleanupCharacters(workSheet.Cells[row, EnglishColumnIndex].Text);
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
			string[] files = FindFiles(code);
			foreach (string file in files)
			{
				if (file.EndsWith("xliff"))
				{
					// code.xliff
					readStringsFromXliff(file, code, readSource, stringsFromLanguage);
				}
				else if (file.EndsWith("InfoPlist.strings"))
				{
					// string to var
					// id = "text";
					// InfoPlist.strings
					readStringsFromStrings(file, code, readSource, stringsFromLanguage, "^(?<id>\\S*)\\s*=\\s*\"(?<val>.*)\";$");
				}
				else
				{
					// string to string
					// "id" = "text";
					// Localizable.strings, Root.strings
					readStringsFromStrings(file, code, readSource, stringsFromLanguage, "^\"(?<id>.*)\"\\s*=\\s*\"(?<val>.*)\";$");
				}
				if (iosToExcelInfo.Error != null)
				{
					break;
				}
			}
			return stringsFromLanguage;
		}

		private void readStringsFromStrings(string fileName, string code, bool readSource, Dictionary<string, string> stringsFromLanguage, string pattern)
		{
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
						string source = cleanupCharacters(match.Groups["id"].Value);
						string target = cleanupCharacters(match.Groups["val"].Value);
						string text = String.Empty;

						if (readSource)
						{
							text = replaceAmbiguousCharacters(source);
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
								//throw new Exception("Duplicate key detected \"" + source + "\"");
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
			catch (Exception ex)
			{
				iosToExcelInfo.Error = new Exception("Reading strings from \"" + fileName + "\"", ex);
			}
		}

		private void readStringsFromXliff(string fileName, string code, bool readSource, Dictionary<string, string> stringsFromLanguage)
		{
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
							target = cleanupCharacters(child.InnerText);
						}
						else if (child.Name == "source")
						{
							source = cleanupCharacters(child.InnerText);
						}
					}

					if (readSource)
					{
						text = replaceAmbiguousCharacters(source);
					}
					else if (target != source)
					{
						text = target;
					}

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
				iosToExcelInfo.Error = new Exception("Reading strings from \"" + fileName + "\"", ex);
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

		private string replaceAmbiguousCharacters(string text)
		{
			// quotations
			text = text
				.Replace("‘", "\\'")
				.Replace("’", "\\'")
				.Replace("“", "\\\"")
				.Replace("”", "\\\"");
			// dash: 1 "en dash", "2 em dash", "3 horizontal bar"
			text = text
				.Replace("–", "--")
				.Replace("—", "--")
				.Replace("―", "--");
			return text;
		}

		private string cleanupCharacters(string text)
		{
			// clean up
			text = text
				.Trim(new char[] { ' ', (char)160 })
				.Replace("\'", "\\\'")
				.Replace("\\\\", "\\")
				.Replace("\n", "\\n")
				.Replace("\r", "\\r")
				.Replace("\t", "\\t");
			// someone left this on a string...
			if (text.EndsWith("%@"))
			{
				text = text.Remove(text.Length - 2);
			}
			return text;
		}

		/// <summary>
		/// Lists all *.strings and *.xliff
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		private string[] FindFiles(string code)
		{
			Stack<string> dirs = new Stack<string>(Directory.GetDirectories(FolderName));
			Stack<string> files = new Stack<string>();
			while (dirs.Count > 0)
			{
				string dir = dirs.Pop();
				if (!dir.EndsWith(".lproj"))
				{
					string[] subDirs = Directory.GetDirectories(dir);
					foreach (string d in subDirs)
					{
						dirs.Push(d);
					}
				}
				string[] dirFiles = Directory.GetFiles(dir);
				if (dirFiles.Length > 0)
				{
					foreach (string dirFile in dirFiles)
					{
						if ((dirFile.EndsWith(".strings") && dirFile.Contains(code + ".lproj")) || dirFile.EndsWith(code + ".xliff"))
						{
							files.Push(dirFile);
							break;
						}
					}
				}
			}
			return files.ToArray();
		}
	}
}