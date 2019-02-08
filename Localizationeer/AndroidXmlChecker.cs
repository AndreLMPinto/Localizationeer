using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Localizationeer
{
	public class AndroidXmlChecker
	{
		public AndroidXmlChecker()
		{
		}

		public string FolderName { get; set; }

		public class AndroidXmlInfo
		{
			public AndroidXmlInfo()
			{
			}

			public List<string> Codes = new List<string>();
			public List<StringData> Summary = new List<StringData>();
			public Exception Error;

			public void AddSummary(string id, string code, string text, bool isTranslatable, string fileName)
			{
				StringData sd = null;
				if (Summary.Exists(d => d.StringId == id))
				{
					sd = Summary.Find(d => d.StringId == id);
				}
				if (sd == null)
				{
					sd = new StringData(id);
					Summary.Add(sd);
				}

				sd.IsTranslatable &= isTranslatable;

				LanguageData ld = null;
				if (sd.Data.Exists(d => d.Code == code))
				{
					ld = sd.Data.Find(d => d.Code == code);
				}
				if (ld == null)
				{
					ld = new LanguageData(code, text, fileName);
					sd.Data.Add(ld);
				}
				else if (ld.Text != text || ld.FileName != fileName)
				{
					throw new Exception("Duplicated text for the same String ID and Code." +
						"\nString ID: " + id +
						"\nCode: " + code +
						"\nText 1 (" + ld.FileName + "): " + ld.Text +
						"\nText 2 (" + fileName + "): " + text);
				}
			}

			public void Validate(ProgressBar bar)
			{
				foreach(StringData sd in Summary)
				{
					sd.Validate(bar, Codes);
				}
			}
		}

		public class StringData
		{
			public StringData(string stringId)
			{
				StringId = stringId;
				IsTranslatable = true;
				Data = new List<LanguageData>();
				FormatIssueCodes = new List<string>();
				MissingTranslationCodes = new List<string>();
				FoundTranslationCodes = new List<string>();
			}

			public string StringId { get; private set; }
			public bool IsTranslatable { get; set; }
			public bool HasDefault { get { return Data.Exists(d => string.IsNullOrEmpty(d.Code)); } }
			public LanguageData DefaultData { get { return Data.Find(d => string.IsNullOrEmpty(d.Code)); } }
			public List<LanguageData> Data { get; private set; }
			public bool FormatIssue { get { return FormatIssueCodes.Count > 0; } }
			public List<string> FormatIssueCodes { get; private set; }
			public bool MissingTranslation { get { return MissingTranslationCodes.Count > 0; } }
			public List<string> MissingTranslationCodes { get; private set; }
			public bool FoundTranslation { get { return FoundTranslationCodes.Count > 0; } }
			public List<string> FoundTranslationCodes { get; private set; }

			public void Validate(ProgressBar bar, List<string> codes)
			{
				// missing/found translations
				if (codes.Count != Data.Count)
				{
					foreach (string code in codes)
					{
						if (!Data.Exists(d => d.Code == code))
						{
							MissingTranslationCodes.Add(code);
						}
						else if (!String.IsNullOrEmpty(code))
						{
							FoundTranslationCodes.Add(code);
						}
					}
				}

				// formatting messed up
				if (Data.Exists(d => string.IsNullOrEmpty(d.Code)))
				{
					LanguageData en = Data.Find(d => string.IsNullOrEmpty(d.Code));
					foreach(LanguageData d in Data)
					{
						// compares the number of specifiers with other languages
						if(!string.IsNullOrEmpty(d.Code))
						{
							if(en.Specifiers.Count != d.Specifiers.Count)
							{
								FormatIssueCodes.Add(d.Code);
							}
						}
					}
				}
			}
		}

		public class LanguageData
		{
			public LanguageData(string code, string text, string fileName)
			{
				Specifiers = new List<string>();
				Code = code;
				Text = text;
				FileName = fileName;
			}

			string _text;

			public string Code { get; private set; }
			public string Text
			{
				get { return _text; }
				private set
				{
					_text = value;
					Specifiers.Clear();
					Regex regex = new Regex("%(\\d\\$)*[ds]");
					foreach(Match match in regex.Matches(_text))
					{
						if (match.Success)
						{
							Specifiers.Add(match.Value);
						}
					}
				}
			}
			public List<string> Specifiers { get; private set; }
			public string FileName { get; private set; }
		}

		public AndroidXmlInfo Validate(ProgressBar bar)
		{
			AndroidXmlInfo info = new AndroidXmlInfo();
			foreach(string key in Constants.AndroidLanguageToCode.Keys)
			{
				foreach(string code in Constants.AndroidLanguageToCode[key].Split(','))
				{
					info.Codes.Add(code);
				}
			}
			try
			{
				foreach (string code in info.Codes)
				{
					string[] fileNames = GetFiles(code);
					foreach (string fileName in fileNames)
					{
						XmlDocument doc = new XmlDocument();
						doc.PreserveWhitespace = true;
						doc.Load(fileName);

						XmlNodeList nodes = doc.SelectNodes("/resources/string");
						foreach (XmlNode node in nodes)
						{
							string id = node.Attributes["name"].Value;
							string text = node.InnerXml;
							bool isTranslatable = true;
							if (node.Attributes["translatable"] != null)
							{
								isTranslatable = node.Attributes["translatable"].Value == "true";
							}
							info.AddSummary(id, code, text, isTranslatable, fileName);
						}
					}
				}
			}
			catch(Exception e)
			{
				info.Error = e;
			}

			if(info.Error == null)
			{
				info.Validate(bar);
			}

			return info;
		}

		private string[] GetFiles(string code)
		{
			string path = Path.Combine(FolderName, "values" + (code == String.Empty ? String.Empty : "-" + code) + "\\");
			return Directory.GetFiles(path, "*.xml");
		}

		public class DeleteKeyInfo
		{
			public DeleteKeyInfo()
			{

			}

			public int Count = 0;
			public Exception Error;
		}

		public DeleteKeyInfo DeleteKey(StringData stringData, bool translationsOnly, ProgressBar bar)
		{
			DeleteKeyInfo info = new DeleteKeyInfo();
			try
			{
				foreach (LanguageData languageData in stringData.Data)
				{
					string code = languageData.Code;
					if(translationsOnly && String.IsNullOrEmpty(code))
					{
						continue;
					}

					string fileName = languageData.FileName;

					XmlDocument doc = new XmlDocument();
					doc.PreserveWhitespace = true;
					doc.Load(fileName);

					XmlNode parent = doc.SelectSingleNode("/resources");
					XmlNodeList nodes = doc.SelectNodes("/resources/string[@name='" + stringData.StringId + "']");
					foreach (XmlNode node in nodes)
					{
						XmlNode previous = node.PreviousSibling;
						while(previous != null && previous is XmlWhitespace)
						{
							XmlNode whitespace = previous;
							previous = whitespace.PreviousSibling;
							parent.RemoveChild(whitespace);
						}
						parent.RemoveChild(node);
						info.Count++;
					}

					doc.Save(fileName);
				}
			}
			catch (Exception e)
			{
				info.Error = e;
			}

			return info;
		}

		/// <summary>
		/// Replaces the content of a string, in every language, using a function.
		/// Given a string A, in the language L, we get the string A', 
		/// which is the result of calling the function F with A as parameter.
		/// </summary>
		/// <example>
		/// ReplaceContent("A", (content) => { return content.Replace("\\r", ""); )
		/// </example>
		/// <param name="stringId">The stringId to be affected by the function.</param>
		/// <param name="newContent">A function that receives a string for the stringId for one language and returns a new string to be used instead.</param>
		/// <returns>The number of effective (languages affected by) changes</returns>
		public int ReplaceContent(string stringId, Func<string, string> newContent)
		{
			int count = 0;
			List<string> codes = new List<string>();
			foreach (string key in Constants.AndroidLanguageToCode.Keys)
			{
				foreach (string code in Constants.AndroidLanguageToCode[key].Split(','))
				{
					codes.Add(code);
				}
			}
			try
			{
				foreach (string code in codes)
				{
					string[] fileNames = GetFiles(code);
					foreach (string fileName in fileNames)
					{
						XmlDocument doc = new XmlDocument();
						doc.PreserveWhitespace = true;
						doc.Load(fileName);

						int changes = 0;
						XmlNodeList nodes = doc.SelectNodes("/resources/string[@name='" + stringId + "']");
						foreach (XmlNode node in nodes)
						{
							string content = newContent(node.InnerXml);
							if (node.InnerXml != content)
							{
								node.InnerXml = content;
								changes++;
							}
						}

						if (changes > 0)
						{
							doc.Save(fileName);
						}
						count += changes;
					}
				}
			}
			catch (Exception)
			{
				return count;
			}

			return count;
		}
	}
}
