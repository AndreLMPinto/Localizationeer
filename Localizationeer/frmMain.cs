using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Localizationeer
{
    public partial class frmMain : Form
    {
        string fileName;
        string folderName;

        Dictionary<String, String> languageToCode = new Dictionary<string, string>()
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

        public frmMain()
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(Properties.Settings.Default.ValuesFolder))
            {
                folderName = Properties.Settings.Default.ValuesFolder;
            }
            tbxSelectedFolder.Text = folderName;
            if (!String.IsNullOrEmpty(Properties.Settings.Default.ExcelFile))
            {
                fileName = Properties.Settings.Default.ExcelFile;
            }
            tbxSelectedFile.Text = fileName;
            updateApplyButton();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (fbdSelectFolder.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(fbdSelectFolder.SelectedPath))
            {
                folderName = fbdSelectFolder.SelectedPath;
                tbxSelectedFolder.Text = folderName;
                updateApplyButton();

                Properties.Settings.Default.ValuesFolder = folderName;
                Properties.Settings.Default.Save();
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (oflSelectFile.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(oflSelectFile.FileName))
            {
                fileName = oflSelectFile.FileName;
                tbxSelectedFile.Text = fileName;
                updateApplyButton();

                Properties.Settings.Default.ExcelFile = fileName;
                Properties.Settings.Default.Save();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            readExcelAndApplyNewValues();
        }

        private void updateApplyButton()
        {
            btnApply.Enabled = !String.IsNullOrEmpty(folderName) && !String.IsNullOrEmpty(fileName);
        }

        private void readExcelAndApplyNewValues()
        {
            int changes = 0;

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(fileName)))
                {
                    var workSheet = xlPackage.Workbook.Worksheets.First();
                    var totalRows = workSheet.Dimension.End.Row;
                    var totalCols = workSheet.Dimension.End.Column;

                    txtOutput.Text = String.Empty;

                    for (int col = 2; col <= totalCols; col++)
                    {
                        var language = workSheet.Cells[1, col].Text;
                        if (languageToCode.ContainsKey(language))
                        {
                            var languageCode = languageToCode[language];

                            Dictionary<string, string> values = new Dictionary<string, string>();

                            for (int row = 2; row <= totalRows; row++)
                            {
                                var id = workSheet.Cells[row, 1].Text;
                                if (id != String.Empty)
                                {
                                    var value = workSheet.Cells[row, col].Text.Trim(new char[] { ' ', (char)160 }).Replace("\'", "\\\'").Replace("\\\\", "\\");
                                    if (values.ContainsKey(id))
                                    {
                                        MessageBox.Show("Duplicate key detected \"" + id + "\".\nPlease review your Excel file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    values.Add(id, value);
                                }
                            }

                            if (values.Count > 0)
                            {
                                string[] languageCodes = languageCode.Split(',');
                                foreach (string code in languageCodes)
                                {
                                    string fileName = Path.Combine(folderName, "values" + (code == String.Empty ? String.Empty : "-" + code) + "\\strings.xml");

                                    logAppendNewLine(language + (code == String.Empty ? String.Empty : " (" + code + ")"));

                                    changes += setValuesInXml(values, fileName);
                                }
                            }
                        }
                    }
                }
                MessageBox.Show("Done: " + changes + " string ids modified");
            }
            catch (IOException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int setValuesInXml(Dictionary<string, string> values, string fileName)
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

                logAppend(": " + count);
            }
            catch(Exception e)
            {
                if(count > 0)
                {
                    logAppend(" aborted");
                }
                logAppendNewLine("  ERROR: " + e.ToString());
            }

            return count;
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

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("He was tired of seeing people do this manually,\nso out of his laziness he made the computer do\nit for them.", "About this application", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
