using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimatorController : MonoBehaviour
{
    private const string BUILDING_EFFECT_NAME = "VFX_Puff";
    private const string BUILDING_UNIT_EFFECT_NAME = "VFX_Unit_Spawn";
    private const string BUILDING_SOUND_NAME = "SND_Build";

    [SerializeField] public GameObject unit;
    private Animator animator;
    public EffectHolder effects;
    public MusicHolder sounds;

    private void Start()
    {
        animator = GetComponent<Animator>();
        effects = GameObject.Find("EffectHolder").GetComponent<EffectHolder>();
        sounds = GameObject.Find("MusicHolder").GetComponent<MusicHolder>();
    }

    public void StartBuildingEffects()
    {
        StartCoroutine(BuildEffectsStarter(1));
    }

    IEnumerator BuildEffectsStarter(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject _effect = Instantiate(effects.GetEffect(BUILDING_EFFECT_NAME), gameObject.transform);
            Debug.Log("[Gameplay] [VFX] Started effect: %" + BUILDING_EFFECT_NAME +"%!");
            Vector3 _newPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            _effect.transform.position = _newPosition;
            _effect.GetComponent<SpriteRenderer>().sortingOrder = 3;
           _effect.GetComponent<EffectsController>().PlaySound(BUILDING_SOUND_NAME);
            Debug.Log("[Gameplay] [Sounds] Started Sound: %" + BUILDING_SOUND_NAME + "%");
            yield return new WaitForSeconds(0.8f);
        }
    }

    public void SpawnUnit()
    {
        if (unit != null)
        {
            Instantiate(effects.GetEffect(BUILDING_UNIT_EFFECT_NAME), unit.transform);
            Debug.Log("[Gameplay] [VFX] Started effect: %" + BUILDING_UNIT_EFFECT_NAME + "%");
            unit.SetActive(true);
            Debug.Log("[Gameplay] [Towers] Spawned Unit: %" + unit.name + "%. For Tower: %" + gameObject.GetComponent<Tower>().towerName + "%");
        }
        else
        {
            Debug.LogError("[!] [Gameplay] [Towers] Cant Find Unit For Tower: %" + gameObject.GetComponent<Tower>().towerName + "%");
        }
        
    }

    public void Builded()
    {
        animator.SetBool("Builded", true);
        animator.SetBool("Placed", false);
        SpawnUnit();
    }
}
