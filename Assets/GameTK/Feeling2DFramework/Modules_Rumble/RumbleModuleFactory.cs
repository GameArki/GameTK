using GameFunctions;

namespace GameTK.Modules_Rumble {

    internal static class RumbleModuleFactory {

        internal static RumbleModuleTaskModel CreateRumbleModuleTaskModel(RumbleMotorType RumbleMotorType, float delay, float startFreq, float endFreq, float duration, GFEasingEnum easingType) {
            return new RumbleModuleTaskModel {
                RumbleMotorType = RumbleMotorType,
                delay = delay,
                startFreq = startFreq,
                endFreq = endFreq,
                duration = duration,
                easingType = easingType,
            };
        }

    }

}