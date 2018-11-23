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

			public void AddSummary(string id, string code, string text, bool isTranslatable)
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

				sd.IsTranslatable = isTranslatable;

				LanguageData ld = null;
				if (sd.Data.Exists(d => d.Code == code))
				{
					ld = sd.Data.Find(d => d.Code == code);
				}
				if (ld == null)
				{
					ld = new LanguageData(code, text);
					sd.Data.Add(ld);
				}
				else if (ld.Text != text)
				{
					throw new Exception("Duplicated text for the same String ID and Code." +
						"\nString ID: " + id +
						"\nCode: " + code +
						"\nText 1: " + ld.Text +
						"\nText 2: " + text);
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
			}

			public string StringId { get; private set; }
			public bool IsTranslatable { get; set; }
			public List<LanguageData> Data { get; private set; }
			public bool FormatIssue { get { return FormatIssueCodes.Count > 0; } }
			public List<string> FormatIssueCodes { get; private set; }
			public bool MissingTranslation { get { return MissingTranslationCodes.Count > 0; } }
			public List<string> MissingTranslationCodes { get; private set; }

			public void Validate(ProgressBar bar, List<string> codes)
			{
				// missing translations
				if(codes.Count != Data.Count)
				{
					foreach(string code in codes)
					{
						if(!Data.Exists(d => d.Code == code))
						{
							MissingTranslationCodes.Add(code);
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
			public LanguageData(string code, string text)
			{
				Specifiers = new List<string>();
				Code = code;
				Text = text;
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
					string fileName = Path.Combine(FolderName, "values" + (code == String.Empty ? String.Empty : "-" + code) + "\\strings.xml");

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
						info.AddSummary(id, code, text, isTranslatable);
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
	}
}
