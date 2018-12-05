# Localizationeer
- Import localization strings from one Excel file to Android XML string files
- Export localization strings from iOS XLIFF string files to one Excel file
- Check for inconsistencies on Android XML string files, looking for missing translations and issues on formattable strings

## Excel to Android
See Example folder for one example of the input file.
- Excel file.
- First column is for the string id.
- Remaining columns are for the strings.
- First line have the names of the languages, which is mapped to one or more language codes (see source).

You can add more lines with your own strings or more columns if you need more languages.

## iOS to Excel
Use the same excel file for input.
You will need 1 column with unique ids and 1 column with the texts in English.
Put all your .XLIFF files in the same folder (or somehow format the path).
Assumes that any .XLIFF file will have all the source (English) strings.
Output is a copy of the input file with the translations the tool managed to find.

## Check Android XML files
Select the folder with yout values*/strings.xml files.
List missing translations: string id exists for english but not for one or more of the other languages.
List issues on formattable strings: string in english has formatting placeholders (%s, %1$s, etc) but not for one or more of the other languages.

## For more languages
If you want to add more languages, add the respective columns with whatever title you want, then add the respective title to Android's language code mapping in the source code.
