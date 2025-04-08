using System.Collections.Generic;
using System.Diagnostics;

namespace NJM {

    public class L10NLangEntity {

        public L10NLangType langType;

        Dictionary<int, string> staticTextDict;
        Dictionary<ulong, string> dialogueTextNewDict;
        Dictionary<int, string> knowledgeTextDict;
        Dictionary<int, string> achievementTextDict;
        Dictionary<int, string> staffTextDict;
        Dictionary<int, string> stuffTextDict;
        Dictionary<int, string> doodleTextDict;
        Dictionary<string, string> symbolTextDict;

        public L10NLangEntity() {
            staticTextDict = new Dictionary<int, string>(1000);
            dialogueTextNewDict = new Dictionary<ulong, string>(1000);
            knowledgeTextDict = new Dictionary<int, string>(1000);
            achievementTextDict = new Dictionary<int, string>(200);
            staffTextDict = new Dictionary<int, string>(100);
            stuffTextDict = new Dictionary<int, string>(100);
            doodleTextDict = new Dictionary<int, string>(1000);
            symbolTextDict = new Dictionary<string, string>(100);
        }

        #region Static
        public void Static_Add(int key, string value) {
            bool succ = staticTextDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Static_TryGet(int key, out string value) {
            return staticTextDict.TryGetValue(key, out value);
        }
        #endregion

        #region Dialogue
        public void Dialogue_Add(int dialogueTypeID, short sentenceIndex, sbyte optionIndex, string value) {
            ulong key = Dialogue_Key(dialogueTypeID, sentenceIndex, optionIndex);
            bool succ = dialogueTextNewDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Dialogue_TryGet(int dialogueTypeID, short sentenceIndex, sbyte optionIndex, out string value) {
            ulong key = Dialogue_Key(dialogueTypeID, sentenceIndex, optionIndex);
            return dialogueTextNewDict.TryGetValue(key, out value);
        }

        ulong Dialogue_Key(int dialogueTypeID, short sentenceIndex, sbyte optionIndex) {
            return (ulong)dialogueTypeID << 32 | (ulong)(ushort)sentenceIndex << 8 | (ulong)(byte)optionIndex;
        }
        #endregion

        #region Knowledge
        public void Knowledge_Add(int key, string value) {
            bool succ = knowledgeTextDict.TryAdd(key, value);
        }

        public bool Knowledge_TryGet(int key, out string value) {
            return knowledgeTextDict.TryGetValue(key, out value);
        }
        #endregion

        #region Achievement
        public void Achievement_Add(int key, string value) {
            bool succ = achievementTextDict.TryAdd(key, value);
        }

        public bool Achievement_TryGet(int key, out string value) {
            return achievementTextDict.TryGetValue(key, out value);
        }

        public bool Achievement_TryGetDesc(int key, out string value) {
            return achievementTextDict.TryGetValue(key + 1, out value);
        }
        #endregion

        #region Staff
        public void Staff_Add(int key, string value) {
            bool succ = staffTextDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Staff_TryGet(int key, out string value) {
            return staffTextDict.TryGetValue(key, out value);
        }
        #endregion

        #region Stuff
        public void Stuff_Add(int key, string value) {
            bool succ = stuffTextDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Stuff_TryGet(int key, out string value) {
            return stuffTextDict.TryGetValue(key, out value);
        }
        #endregion

        #region Doodle
        public void Doodle_Add(int key, string value) {
            bool succ = doodleTextDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Doodle_TryGet(int key, out string value) {
            return doodleTextDict.TryGetValue(key, out value);
        }
        #endregion

        #region Symbol
        public void Symbol_Add(string key, string value) {
            bool succ = symbolTextDict.TryAdd(key, value);
            if (!succ) {
                // NJLog.Warning($"SameKey, {key}: {value}");
            }
        }

        public bool Symbol_TryGet(string key, out string value) {
            return symbolTextDict.TryGetValue(key, out value);
        }
        #endregion
    }

}