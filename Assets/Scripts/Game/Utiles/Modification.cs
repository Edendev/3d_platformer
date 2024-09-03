namespace Game.Utiles
{
    public abstract class Modification<T>
    {
        public abstract T Modify(T target, T value);
    }

    public class AddFloatModification : Modification<float>
    {
        public override float Modify(float target, float value) {
            return target + value;
        }
    }

    public class SubstractFloatModification : Modification<float>
    {
        public override float Modify(float target, float value) {
            return target - value;
        }
    }

    public class MultiplyFloatModification : Modification<float>
    {
        public override float Modify(float target, float value) {
            return target * value;
        }
    }

    public class DivideFloatModification : Modification<float>
    {
        public override float Modify(float target, float value) {
            return target / value;
        }
    }
}
