using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {
    public RankField playerRankField;
    public List<RankField> enemyRankFields;
    public List<GameObject> SliderPositions;
    public Image damageImage;
    public ManagerScript _manager;
    public float flashSpeed = 3f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

    public bool damaged = false;

    private void Update()
    {
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            damageImage.color = flashColour;
            damaged = false;
        }
        else
        {
            // ... transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
    }

    public void UpdateHealth(int value, GameObject character)
    {
        if (_manager.player == character)
        {
            damaged = true;
            if (value <= 0)
            {
                playerRankField.slider.value = character.GetComponent<CarGeneral>().maxHealth;
                Resurrection(character);
            } else
            {
                playerRankField.slider.value = value;
            }
        }
        else
        {
            for (int i = 0; i < enemyRankFields.Count; ++i)
            {
                if (character == _manager.enemies.transform.GetChild(i).gameObject)
                {
                    if (value <= 0)
                    {
                        enemyRankFields[i].slider.value = character.GetComponent<CarGeneral>().maxHealth;
                        Resurrection(character);
                    }
                    else
                    {
                        enemyRankFields[i].slider.value = value;
                    }
                }
            }
        }
    }

    public void Resurrection(GameObject character)
    {
        character.GetComponent<CarGeneral>().FlipCar();
        character.GetComponent<CarGeneral>().CurFine += _manager.DeathFine;
        character.GetComponent<CarGeneral>().currentHealth = character.GetComponent<CarGeneral>().maxHealth;
    }

    public void UpdateRanks(List<GameObject> ranks)
    {
        for (int i = 0; i < ranks.Count; ++i)
        {
            if (ranks[i] == _manager.player)
            {
                playerRankField.transform.position = SliderPositions[ranks.Count - i - 1].transform.position;
            } else
            {
                for (int j = 0; j < enemyRankFields.Count; ++j)
                {
                    if (ranks[i] == _manager.enemies.transform.GetChild(j).gameObject)
                    {
                        enemyRankFields[j].transform.position = SliderPositions[ranks.Count - i - 1].transform.position;
                        break;
                    }
                }
            }
        }
    }
}
