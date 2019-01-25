using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Habtic.Games.Colr
{
	public class WordDictionary : Singleton<WordDictionary>
	{
		public TextAsset wordListFile;
		public TextAsset goodAnswerTextsFile;
		public TextAsset wrongAnswerTextsFile;
		private List<string> shortWords = new List<string>();
		private List<string> longWords = new List<string>();
		private List<string> goodAnswerTexts = new List<string>();
		private List<string> wrongAnswerTexts = new List<string>();

		void Start()
		{
			ReadTexts(wordListFile, null, true);
			ReadTexts(goodAnswerTextsFile, goodAnswerTexts);
			ReadTexts(wrongAnswerTextsFile, wrongAnswerTexts);
		}

		private void ReadTexts(TextAsset txtFile, List<string> list, bool split = false) { 
			string line;
			int i = 0;
			StringReader theReader = new StringReader(txtFile.text);
			using (theReader)
			{
				do
				{
					line = theReader.ReadLine();

					if (line != null)
					{
						if (line.Length > 0)
						{
							if (split && list == null)
							{
								if (line.Length > 5)
									longWords.Add(line);
								else
									shortWords.Add(line);
							}else
							{
								list.Add(line);
							}
							i++;
						}
					}
				}
				while (line != null);
				// Done reading, close the reader and return true to broadcast success    
				theReader.Close();
			}
		}

		public string GetShortWord()
		{
			return shortWords[Random.Range(0, shortWords.Count)].ToUpper();
		}

		public string GetLongWord()
		{
			return longWords[Random.Range(0, longWords.Count)].ToUpper();
		}

		public string GetGoodAnswerText
		{
			get { return goodAnswerTexts[Random.Range(0, goodAnswerTexts.Count)].ToUpper(); }
		}

		public string GetWrongAnswerText
		{
			get { return wrongAnswerTexts[Random.Range(0, wrongAnswerTexts.Count)].ToUpper(); }
		}
	}
}
