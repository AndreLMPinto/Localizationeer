using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Localizationeer
{
	public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
			InitializeControls();
        }

		private void InitializeControls()
		{
			cbxOption.SelectedIndex = Properties.Settings.Default.Action;
			if (!String.IsNullOrEmpty(Properties.Settings.Default.ValuesFolder))
			{
				tbxSelectedFolder.Text = Properties.Settings.Default.ValuesFolder;
			}
			if (!String.IsNullOrEmpty(Properties.Settings.Default.ExcelFile))
			{
				tbxSelectedFile.Text = Properties.Settings.Default.ExcelFile;
			}
			NudID.Value = Properties.Settings.Default.IdColumnIndex;
			NudEnglish.Value = Properties.Settings.Default.EnglishColumnIndex;
			NudThreshold.Value = Properties.Settings.Default.Threshold;
			cbxFilter.SelectedIndex = Properties.Settings.Default.Filter;
			updateButtons();
		}

		private void ResetSettings()
		{
			Properties.Settings.Default.Action = 0;
			Properties.Settings.Default.ValuesFolder = "";
			Properties.Settings.Default.ExcelFile = "";
			// excel col,row count is 1 based
			Properties.Settings.Default.IdColumnIndex = 1;
			Properties.Settings.Default.EnglishColumnIndex = 2;
			Properties.Settings.Default.Threshold = 100;
			Properties.Settings.Default.Filter = 0;
			InitializeControls();
		}

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (fbdSelectFolder.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(fbdSelectFolder.SelectedPath))
            {
				tbxSelectedFolder.Text = fbdSelectFolder.SelectedPath;
            }
		}

		private void tbxSelectedFolder_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.ValuesFolder = tbxSelectedFolder.Text;
			Properties.Settings.Default.Save();
			updateButtons();
		}

		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			if (oflSelectFile.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(oflSelectFile.FileName))
			{
				tbxSelectedFile.Text = oflSelectFile.FileName;
			}
		}

		private void tbxSelectedFile_TextChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.ExcelFile = tbxSelectedFile.Text;
			Properties.Settings.Default.Save();
			updateButtons();
		}

		private void NudID_ValueChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.IdColumnIndex = (int)NudID.Value;
			Properties.Settings.Default.Save();
		}

		private void NudEnglish_ValueChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.EnglishColumnIndex = (int)NudEnglish.Value;
			Properties.Settings.Default.Save();
		}

		private void NudThreshold_ValueChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Threshold = (int)NudThreshold.Value;
			Properties.Settings.Default.Save();
		}

		private void cbxOption_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Action = cbxOption.SelectedIndex;
			Properties.Settings.Default.Save();
			updateButtons();
		}

		private void cbxFilter_SelectedIndexChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.Filter = cbxFilter.SelectedIndex;
			Properties.Settings.Default.Save();
		}

		private void btnAbout_Click(object sender, EventArgs e)
		{
			MessageBox.Show("He was tired of seeing people do this manually,\nso out of his laziness he made the computer do\nit for them.", "About this application", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnReset_Click(object sender, EventArgs e)
		{
			ResetSettings();
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			logClear();
			switch (Properties.Settings.Default.Action)
			{
				case 0:
					excelToAndroid();
					break;
				case 1:
					iosToExcel();
					break;
				case 2:
					checkAndroid();
					break;
			}
		}

		private void excelToAndroid()
		{
			ExcelToAndroid importer = new ExcelToAndroid();
			importer.FileName = Properties.Settings.Default.ExcelFile;
			importer.FolderName = Properties.Settings.Default.ValuesFolder;
			importer.IdColumnIndex = Properties.Settings.Default.IdColumnIndex;
			importer.EnglishColumnIndex = Properties.Settings.Default.EnglishColumnIndex;

			ExcelToAndroid.ExcelToAndroidInfo info = importer.ReadExcelAndApplyNewValues();
			if (info.Error == null)
			{
				foreach (KeyValuePair<string, string> item in info.summary)
				{
					logAppendNewLine(item.Key + ": " + item.Value);
				}
			}
			else
			{
				logException(info.Error);
			}
		}

		private void iosToExcel()
		{
			IosToExcel importer = new IosToExcel();
			importer.FileName = Properties.Settings.Default.ExcelFile;
			importer.FolderName = Properties.Settings.Default.ValuesFolder;
			importer.IdColumnIndex = Properties.Settings.Default.IdColumnIndex;
			importer.EnglishColumnIndex = Properties.Settings.Default.EnglishColumnIndex;
			importer.Threshold = Properties.Settings.Default.Threshold;

			IosToExcel.IosToExcelInfo info = importer.Export(progressBar);
			if (info.Error == null)
			{
				logAppendNewLine("Strings to look for: " + info.stringsToLookFor.Count);
				foreach (KeyValuePair<string, int> item in info.stringsFromLanguage)
				{
					logAppendNewLine("Strings from " + item.Key + ": " + item.Value);
				}
				logAppendNewLine("Strings matching: " + info.stringsMatching.Count);
				foreach (KeyValuePair<string, string> item in info.stringsMatching)
				{
					logAppendNewLine("[" + item.Key + "] (" + info.howClose[item.Key] + "%)");
					if (info.howClose[item.Key] < 100)
					{
						logAppendNewLine(info.stringsToLookFor[item.Key]);
						logAppendNewLine(info.howCloseMatching[item.Key]);
					}
				}
				logAppendNewLine("Saved to: " + info.OutputFileName);
			}
			else
			{
				logException(info.Error);
			}
		}

		private void checkAndroid()
		{
			AndroidXmlChecker checker = new AndroidXmlChecker();
			checker.FolderName = Properties.Settings.Default.ValuesFolder;
			AndroidXmlChecker.AndroidXmlInfo info = checker.Validate(progressBar);

			logClear();
			if (info.Error != null)
			{
				logException(info.Error);
			}
			else
			{
				StringBuilder sb = new StringBuilder();
				int count = 0;
				foreach(AndroidXmlChecker.StringData sd in info.Summary)
				{
					bool showMissingTranslation = sd.MissingTranslation && (Properties.Settings.Default.Filter == 0 || Properties.Settings.Default.Filter == 1);
					bool showFormatIssue = sd.FormatIssue && (Properties.Settings.Default.Filter == 0 || Properties.Settings.Default.Filter == 2);
					if (!showMissingTranslation && !showFormatIssue)
					{
						continue;
					}
					count++;
					sb.Append((sb.Length == 0 ? "" : "\r\n") + sd.StringId);
					//logAppendNewLine(sd.StringId);
					if (showMissingTranslation)
					{
						sb.Append("\r\n    Missing translations (" + sd.MissingTranslationCodes.Count + "): ");
						//logAppendNewLine("    Missing translations (" + sd.ErrorMissingTranslationCodes.Count + "): ");
						for (int i = 0; i < sd.MissingTranslationCodes.Count; i++)
						{
							string code = sd.MissingTranslationCodes[i];
							if(string.IsNullOrEmpty(code))
							{
								code = "en";
							}
							sb.Append((i > 0 ? ", " : "") + code);
							//logAppend((i > 0 ? ", " : "") + code);
						}
					}
					if (showFormatIssue)
					{
						sb.Append("\r\n    Format issues (" + sd.FormatIssueCodes.Count + "): ");
						//logAppendNewLine("    Format issues (" + sd.ErrorSpecifierCodes.Count + "): ");
						for (int i = 0; i < sd.FormatIssueCodes.Count; i++)
						{
							string code = sd.FormatIssueCodes[i];
							if (string.IsNullOrEmpty(code))
							{
								code = "en";
							}
							sb.Append((i > 0 ? ", " : "") + code);
							//logAppend((i > 0 ? ", " : "") + code);
						}
					}
				}
				if(count > 0)
				{
					sb.Append("\r\n==========\r\nString IDs with issues: " + count);
					//logAppendNewLine("==========\r\nString IDs with issues: " + count);
					if(Properties.Settings.Default.Filter == 0 || Properties.Settings.Default.Filter == 1)
					{
						sb.Append("\r\nLanguage codes: " + info.Codes.Count);
						sb.Append("\r\nTips:" +
							"\r\n    Strings without English (en) are probably out of use." +
							"\r\n    Strings that are only in English are probably awaiting for translation or not supposed to be translated.");
					}
				}
				logAppend(sb.ToString());
			}
		}

        private void updateButtons()
        {
			switch (Properties.Settings.Default.Action)
			{
				case 0:
				case 2:
					lblSelectedFolder.Text = "The app/src/main/res folder which contains your values*/strings.xml files:";
					break;
				case 1:
					lblSelectedFolder.Text = "The folder which contains your iOS files:";
					break;
			}

			btnApply.Enabled = !String.IsNullOrEmpty(Properties.Settings.Default.ValuesFolder) && !String.IsNullOrEmpty(Properties.Settings.Default.ExcelFile);
		}

		private void logClear()
		{
			txtOutput.Text = String.Empty;
		}

		private void logAppend(string text)
        {
            txtOutput.Text += text;
            Application.DoEvents();
        }

        private void logAppendNewLine(string text)
        {
            logAppend((txtOutput.Text == String.Empty ? String.Empty : "\r\n") + text);
        }

		private void logException(Exception error)
		{
			logAppendNewLine("Operation failed\r\n" +
				error.Message +
				(error.InnerException != null ? "\r\n" + error.InnerException.ToString() : ""));
		}
	}
}
