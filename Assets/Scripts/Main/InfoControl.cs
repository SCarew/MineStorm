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
		return s.Split('#');
	}

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

	void Level6h() {
		info = "Be aware of substantial asteroid activity in the hyperspace tunnel from sector Beta I to Beta II.  ";
		info = info + "Command is still ascertaining the cause.";
	}

	void Level9() {
		info = "Extremely dangerous mines have been reported in the Beta cluster!  ";
		info = info + "These incorporate the attraction properties of magnetic mines while maintaining the firing charges of electric mines.  ";
	}

	void Level10h() {
		info = "Reports indicate Alaghaz-class (pink) enemy vessels are now harassing ships in hyperspace.  ";
		info = info + "Although they do not have functional weaponry, they are mining hyperspace lanes with asteroids.  ";
		info = info + "Extreme caution is urged.";
	}

	void Level13h() {
		info = "Pilots are now reporting large, extra dense asteroids, starting in the Delta cluster.  ";
		info = info + "These require considerable firepower to destroy.  How these are connected to the enemy's mines is unclear.";
	}

	void Level15h() {
		info = "Please be advised of heavy asteroid activity in the hyperspace tunnel from the Delta cluster to sector Gamma I.  ";
		info = info + "All pilots using this tunnel should be cautious.";
	}

	void Level16() {
		info = "Meteor Buster pilots entering the Gamma cluster for the first time should be aware of poor light conditions.  ";
		info = info + "The distance and low intensity of nearby stars can make asteroid clearing even more treacherous.  ";
		info = info + "Attention should be paid to radar instruments for assistance.";
	}

	void Level17h() {
		info = "Space pirates are now deploying a new, powerful type of mine.  ";
		info = info + "Buster pilots in the Gamma cluster are referring to them as Black Hole Mines.  ";
		info = info + "Strong gravitational fields emanate from these mines, affecting Meteor Busters.";
	}

	void Level21() {
		info = "Remaining Buster pilots are asked to proceed to the Omega cluster on the outskirts of controlled territory.  ";
		info = info + "Pirate activity has become extremely high in these sectors recently.  ";
		info = info + "We have lost several freighters en route to our outer colonies.";
	}

	void Level23() {
		info = "Heavy pirate activity is reported throughout the Omega cluster.  ";
		info = info + "It is believed that the pirates' home base must be nearby.  ";
		info = info + "Clear this area of mines to restore trade routes for our economy.";
	}

	void Level25h() {
		info = "Space pirates are now filling the hyperspace tunnels in the Omega cluster.  ";
		info = info + "Please ensure mining lasers are fully functional if traveling by hyperspace."; 
	}

	void Level26() {
		info = "The final group of space pirates has gathered in sector Omega V.  ";
		info = info + "Clear this small, remaining sector to make our territory free for shipping again!"; 
	}

}
