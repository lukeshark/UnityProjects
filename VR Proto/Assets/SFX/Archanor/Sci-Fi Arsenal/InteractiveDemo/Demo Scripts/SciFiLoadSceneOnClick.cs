using UnityEngine;
using UnityEngine.SceneManagement;

namespace SciFiArsenal
{
public class SciFiLoadSceneOnClick : MonoBehaviour
{
    public void LoadSceneSciFiProjectiles()  {
		SceneManager.LoadScene ("scifi_projectiles");
	}
    public void LoadSceneSciFiBeamup()  {
        SceneManager.LoadScene("scifi_beamup");
	}
    public void LoadSceneSciFiBuff()  {
        SceneManager.LoadScene("scifi_buff");
	}
    public void LoadSceneSciFiFlamethrowers2()  {
		SceneManager.LoadScene ("scifi_flamethrowers");
	}
    public void LoadSceneSciFiQuestZone()  {
        SceneManager.LoadScene ("scifi_hexagonzone");
	}
    public void LoadSceneSciFiLightjump()  {
        SceneManager.LoadScene ("scifi_lightjump");
	}
    public void LoadSceneSciFiLoot()  {
        SceneManager.LoadScene ("scifi_loot");
	}
    public void LoadSceneSciFiBeams()  {
        SceneManager.LoadScene ("scifi_beams");
    }
    public void LoadSceneSciFiPortals()  {
        SceneManager.LoadScene ("scifi_portals");
    }
    public void LoadSceneSciFiRegenerate() {
        SceneManager.LoadScene("scifi_regenerate");
    }
    public void LoadSceneSciFiShields() {
        SceneManager.LoadScene("scifi_shields");
    }
    public void LoadSceneSciFiSwirlyAura() {
        SceneManager.LoadScene("scifi_swirlyaura");
    }
    public void LoadSceneSciFiWarpgates() {
        SceneManager.LoadScene("scifi_warpgates");
    }
    public void LoadSceneSciFiJetflame(){
        SceneManager.LoadScene("scifi_jetflame");
    }
    public void LoadSceneSciFiUltimateNova(){
        SceneManager.LoadScene("scifi_ultimatenova");
    }
}
}