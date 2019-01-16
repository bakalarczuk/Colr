using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Habtic.Games.Colr
{
	public class WordDictionary : Singleton<WordDictionary>
	{
		public TextAsset wordListFile;
		private List<string> shortWords = new List<string>();
		private List<string> longWords = new List<string>();

		void Start()
		{
			string line;
			int i = 0;
			StringReader theReader = new StringReader(wordListFile.text);
			using (theReader)
			{
				do
				{
					line = theReader.ReadLine();

					if (line != null)
					{
						if (line.Length > 0)
						{
							if (line.Length > 5)
								longWords.Add(line);
							else
								shortWords.Add(line);
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
	}
}
