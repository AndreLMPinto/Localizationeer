# Localizationeer
Tool to import localization strings from one Excel file to Android XML string files

## Input file
See Example folder for one example of the input file.
- Excel file.
- First column is for the string id.
- Remaining columns are for the strings.
- First line have the name of the language, which is mapped to one or more language codes (see source).

You can add more lines with your own strings or more columns if you need more languages.

## For more languages
If you want to add more languages, add the respective columns with whatever title you want, then add the respective _title to Android's language code_ mapping in the source code.
