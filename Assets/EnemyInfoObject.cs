using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyInfo", order=1)]
public class EnemyInfoObject : ScriptableObject
{
    public string enemyObject;
    public Sprite enemyPlaceholderSprite;
}
