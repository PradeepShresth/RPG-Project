using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializeVector3
    {
        float x, y, z;

        public SerializeVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x,y,z);
        }
    }
}