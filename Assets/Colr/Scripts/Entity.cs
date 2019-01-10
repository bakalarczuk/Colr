using UnityEngine;

namespace Habtic.Games.Colr
{
    public enum EntityTag
    {
        UNTAGGED,
        COPTER,
        SHIP,
        BIRD,
        HURRICANE,
        TORNADO,
        BONUSPOINT
    }

    public abstract class Entity : MonoBehaviour
    {

        public EntityTag Tag = EntityTag.UNTAGGED;

        [SerializeField]
        protected SpriteRenderer bodyRenderer;

        public virtual Color Tint { get { return bodyRenderer.color; } set { bodyRenderer.color = value; } }

        public abstract void RemoveSelf();
    }
}
