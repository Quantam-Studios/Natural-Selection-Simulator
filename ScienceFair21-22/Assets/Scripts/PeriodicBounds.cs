using UnityEngine;

public class PeriodicBounds : MonoBehaviour
{
    // COLLISION
    private void OnCollisionEnter2D(Collision2D col)
    {
        // Get reference of creature
        GameObject creature = col.gameObject;
        // Calculate new Vector3
        Vector3 newPos = new Vector3(creature.transform.position.x * -1, creature.transform.position.y * -1, 1);
        // Move Creature to the new vector
        creature.transform.position = newPos;
    }
}
