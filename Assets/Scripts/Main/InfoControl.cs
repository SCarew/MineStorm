using System.Collections;
using UnityEngine;

public class InfoControl : MonoBehaviour {

	private string info = "";

	void Start () {
		
	}

	public string GetInfo(int level, bool bHyperspace = false) {  
		//called by computer panel and GetInfoArray()
		info = "";
		if (!bHyperspace) {   //not calling from/about Hyperspace
			if 		(level==1) { Level1(); }
			else if (level==2) { Level2(); }
			else if (level==4) { Level4(); }
		} else {              //calling from/about Hyperspace
			if 		(level==1) { Level1h(); }
			else if (level==4) { Level4h(); }
		}

		return info;
	}

	public string[] GetInfoArray(int level, bool bHyperspace = false) {  
		//called by pause screen
		string s = "";
		string s1 = "", s2 = "";

		while (level > 0) {
			s1 = GetInfo(level, bHyperspace);
			s2 = GetInfo(level - 1, bHyperspace);

			if (s == "")  {        //for initial time through while loop
				if (s1 != "") { s = s + s1 + "#"; }
				//Debug.Log("Should run only once: " + level + " " + bHyperspace + "[" + s + "]");
			} else {
				if (s1 != s2 && s1 != "") { s = s + s1 + "#"; }
			}
			if (!bHyperspace) { level--; }
			bHyperspace = !bHyperspace;
		}
		s = s.Substring(0, s.Length-1);  //remove trailing #
		Debug.Log ("s===[" + s + "]");
		return s.Split('#');
	}

	//TODO Finish level info messages
	void Level1() {
		info = "Asteroids are a problem for trade ships.  Blow up all asteroids until sectors are clear.  ";
		info = info + "Space pirates seem to be taking advantage of the asteroid disruption.";
	}

	void Level1h() {
		info = "When a sector is cleared, your ship's computer will set course to the next sector through hyperspace.  ";
		info = info + "Dimensional variations and the power required for the trip will disable most ship systems.  ";
		info = info + "Although weapons and scanners are offline, direct navigation can be used to pilot around obstacles encountered in hyperspace.  ";
		info = info + "Meteor Busters are equipped with low-power mining lasers that will function if destroying asteroids is necessary.";
	}

	void Level2() {
		info = "The enemy has begun to deploy mines in this sector.  Electric mines carry a charge similar to a torpedo.  ";
		info = info + "When destroyed, this charge is fired at your ship.  Beware.  ";
		info = info + "Larger mines are able to hyperwarp smaller mines into the neighboring area.";
	}

	void Level4() {
		info = "A new type of mine has been reported in your area.  ";
		info = info + "Magnetic mines are attracted to the duranium in our ships' hulls.  ";
	}

	void Level4h() {
		info = "Reports indicate pirate vessels are appearing more frequently in hyperspace.  ";
		info = info + "Their weapon systems should not function any better than ours, but be careful nonetheless.";
	}

	void Level5() {
		info = "New enemy vessels are expected prior to reaching the Beta cluster.  ";
		info = info + "These vessels seem tougher and use torpedoes as their primary weapon.  ";
		info = info + "Meteor Buster pilots are urged to be cautious.";
	}

	void Level6() {
		info = "Sectors in the Beta cluster are known to have a wide variety of mines and asteroids.  ";
		info = info + "Space pirates are increasing their presence in the area.  ";
		info = info + "Trade freighters will be in extreme danger if Meteor Busters can't clear the area.";
	}

	void Level9() {
		info = "Extremely dangerous mines have been reported in the Beta cluster!  ";
		info = info + "These incorporate the attraction properties of magnetic mines while maintaining the firing charges of electric mines.  ";
	}

	void Level16() {
		info = "Meteor Buster pilots entering the Gamma cluster for the first time should be aware of poor light conditions.  ";
		info = info + "The distance and low intensity of nearby stars can make asteroid clearing even more treacherous.  ";
		info = info + "Attention should be paid to radar instruments for assistance.";
	}


}
