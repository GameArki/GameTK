using System;

namespace GameClasses.SpriteAnimatorLib {

    [Flags]
    public enum SpriteAnimatorEqualLogicType {
        None,
        Equal = 1 << 0,
        GreaterThanCondition = 1 << 1,
        LessThanCondition = 1 << 2,
    }

}