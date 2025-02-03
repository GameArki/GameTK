using System.Collections.Generic;
using UnityEngine;
using GameFunctions;

namespace NJM.Modules_Sound {

    public class SoundModuleContext {

        public SoundModuleProcessEntity processEntity;
        public SoundModuleRepo repo;
        public SoundModuleTemplate template;
        public Transform poolRoot;
        public Pool<SoundModuleEntity> pool_entity;

        public int idRecord;

        public SoundModuleContext() {
            processEntity = new SoundModuleProcessEntity();
            repo = new SoundModuleRepo();
            template = new SoundModuleTemplate();
            idRecord = 1;
        }

        public void Inject(Transform poolRoot) {
            this.poolRoot = poolRoot;
        }

    }

}