using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Enemy", order = 1)]
public class Enemy_ScriptableObject : ScriptableObject
{
    [Header("Enemy Parameters")]
    public string enemyName; // Имя врага
    public float accelaration = 120; // Ускорение врага, как быстро он может развить максимальную скорость измерения метры в секунду
    public int angelarSpeed = 120; // Угол поворота врага, как быстро он может повернуться измерения градус в секунду

    [Header("Enemy Chase")]
    public float enemyChaseSpeed = 6f; // Скорость преследования
    public float enemyLostPlayerTime = 5f; // Время потери игрока
    
    [Header("Enemy Patroling")]
    public float enemyPatrolingSpeed = 4f; // Скорость патрулирования
    public float enemyPatrolingWaitTime = 2f; // Время ожидания при патрулировании
    
    [Header("Enemy Searching")]
    public float enemySearchingSpeed = 3f; // Скорость поиска
    public float enemySearchingDuration = 2f; // Время ожидания при поиске
    public float enemySearchRadius = 5f; // Радиус поиска
    public float offsetSearchingPoint = 10f; // Радиус области поиска
    
    [Header("Enemy Health")]
    public float enemyHealth = 100f; // Здоровье врага
    
    [Header("Enemy Attack")]
    public float enemyDamage = 100f; // Урон врага
    public float enemyAttackRangeMin = 10f; // Минимальная дальность атаки врага
    public float enemyAttackRangeMax = 15f; // Максимальная дальность атаки врага
    public float enemyAttackDuration = 5f; // Длительность атаки
    public float enemyAttackDelay = 1f; // задержка перед атакой
    public float enemyAttackCooldown = 4f; // Скорость атаки врага


}
