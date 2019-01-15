using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Habtic.Games.Colr
{
	[Serializable]
	public struct ColrColor
	{
		public enum ColorNames
		{
			Lime,
			Green,
			Aqua,
			Blue,
			Navy,
			Purple,
			Pink,
			Red,
			Orange,
			Yellow
		}

		public ColorNames colorName;

		private static Hashtable colourValues = new Hashtable{
		 { ColorNames.Lime,     new Color32( 166 , 254 , 0, 255 ) },
		 { ColorNames.Green,     new Color32( 0 , 254 , 111, 255 ) },
		 { ColorNames.Aqua,     new Color32( 0 , 201 , 254, 255 ) },
		 { ColorNames.Blue,     new Color32( 0 , 122 , 254, 255 ) },
		 { ColorNames.Navy,     new Color32( 60 , 0 , 254, 255 ) },
		 { ColorNames.Purple, new Color32( 143 , 0 , 254, 255 ) },
		 { ColorNames.Pink,     new Color32( 232 , 0 , 254, 255 ) },
		 { ColorNames.Red,     new Color32( 254 , 9 , 0, 255 ) },
		 { ColorNames.Orange, new Color32( 254 , 161 , 0, 255 ) },
		 { ColorNames.Yellow, new Color32( 254 , 224 , 0,255 ) },
	 };

		public static Color32 ColourValue(ColorNames color)
		{
			return (Color32)colourValues[color];
		}  
	}
}
