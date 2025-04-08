using System;
using GameFunctions;

namespace GameTK {

    internal class RumbleModuleEntity {

        internal float startFreq;
        internal float endFreq;
        internal float duration;
        internal GFEasingEnum easingType;

        internal float currentTime;
        internal bool isFinished;

        internal float currentFreq;

        internal void UpdateRumbleFromModel(RumbleModuleTaskModel model) {
            this.startFreq = model.startFreq;
            this.endFreq = model.endFreq;
            this.duration = model.duration;
            this.easingType = model.easingType;

            this.currentTime = 0;
            this.isFinished = false;
            this.currentFreq = model.startFreq;
        }

    }

}