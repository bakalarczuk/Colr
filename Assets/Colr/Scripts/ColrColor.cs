using System;
using System.Collections;
using System.Collections.Generic;
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
			Red,
			Orange,
			Yellow,
			Olive,
			Green,
			Purple,
			Fuchsia,
			Lime,
			Aqua,
			Blue,
			Navy,
			Black,
			Gray,
			Silver,
			White,
			Pink
		}

		public ColorNames colorName;

		private static Hashtable colourValues = new Hashtable{
		 { ColorNames.Red,		new Color32( 255, 0, 0, 255 ) },
		 { ColorNames.Orange,	new Color32( 255, 165, 0, 255 ) },
		 { ColorNames.Yellow,	new Color32( 255, 255, 0, 255 ) },
		 { ColorNames.Olive,	new Color32( 128, 128, 0, 255 ) },
		 { ColorNames.Green,	new Color32( 0, 128, 0, 255 ) },
		 { ColorNames.Purple, 	new Color32( 128, 0, 128, 255 ) },
		 { ColorNames.Fuchsia, 	new Color32( 255, 0, 255, 255 ) },
		 { ColorNames.Lime,		new Color32( 0, 255, 0, 255 ) },
		 { ColorNames.Aqua,		new Color32( 0, 255, 255, 255 ) },
		 { ColorNames.Blue,		new Color32( 0, 0, 255, 255 ) },
		 { ColorNames.Navy,		new Color32( 0, 0, 128, 255 ) },
		 { ColorNames.Black,	new Color32( 0, 0, 0, 255 ) },
		 { ColorNames.Gray,		new Color32( 128, 128, 128, 255 ) },
		 { ColorNames.Silver,	new Color32( 192, 192, 192, 255 ) },
		 { ColorNames.White,	new Color32( 255, 255, 255, 255 ) },
		 { ColorNames.Pink,		new Color32( 255, 105, 180, 255 ) }
	 };

		public static Color32 ColourValue(ColorNames color)
		{
			return (Color32)colourValues[color];
		}  
	}
}
