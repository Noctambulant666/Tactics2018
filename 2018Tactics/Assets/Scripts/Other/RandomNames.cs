using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class RandomNames {
	public static string RandomName(){
		string path = Application.dataPath+"/names.txt";
		if ( File.Exists(path) ){
			string[] lines = File.ReadAllLines(path);
			return lines[ Random.Range( 0, lines.Length-1 ) ];
		}
		else return "Buddy";
	}
	public static string RandomName2(){
		string[] names = new string[]{
			"Steve",
			"Chris",
			"Katie",
			"Bill",
			"Dakota",
			"Bort",
			"Burt",
			"Bart",
			"Liam",
			"Jakob",
			"Hannah",
			"Carlos",
			"Jane",
			"Dave",
			"Mrs. Picken, whose name I have forgotten",
			"Leita",
			"Tony",
			"Louis",
			"Zak",
			"Natasha",
			"Kelly",
			"Lisa",
			"Ashton",
			"Margaret",
			"Homer",
			"Elvis",
			"Justin",
			"Kurt"
		};

		string name = "Unknown";
		name = names[ Random.Range( 0, names.Length-1) ];

		return name;
	}
}