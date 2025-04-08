using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using NReco.Csv;
using TriInspector;

namespace NJM {

    public class L10NModule : MonoBehaviour {

        [SerializeField] TextAsset csv_misc;
        [SerializeField] TextAsset csv_static;
        [SerializeField] TextAsset csv_doodle;
        [SerializeField] TextAsset csv_dialogue;
        [SerializeField] TextAsset csv_word;
        [SerializeField] TextAsset csv_symbol;

        L10NLangType currentLangType;
        Dictionary<L10NLangType, L10NLangEntity> all;

        // === Temp ====
        Array langTypes;
        string[] langNames;

        public void Ctor() {
            all = new Dictionary<L10NLangType, L10NLangEntity>();

            langTypes = Enum.GetValues(typeof(L10NLangType)); // CN EN JP
            langNames = new string[langTypes.Length]; // CN EN JP
            for (int i = 0; i < langTypes.Length; i += 1) {
                L10NLangType langType = (L10NLangType)langTypes.GetValue(i);
                langNames[i] = langType.ToString();

                L10NLangEntity entity = new L10NLangEntity();
                entity.langType = langType;
                all.Add(langType, entity);
            }

            currentLangType = L10NLangType.ZH_CN;
        }

        public void LoadAll() {
            Normal_Load(csv_static.bytes, (entity, id, text) => {
                entity.Static_Add(id, text);
            });
            Normal_Load(csv_doodle.bytes, (entity, id, text) => {
                entity.Doodle_Add(id, text);
            });
            Word_Load(csv_symbol.bytes, (entity, id, text) => {
                entity.Symbol_Add(id, text);
            });
            Dialogue_Load(csv_dialogue.bytes);
        }

        #region LangType
        public void LangType_Set(L10NLangType langType) {
            currentLangType = langType;
        }

        public L10NLangType LangType_Get() {
            return currentLangType;
        }
        #endregion

        #region Load: Normal
        void Normal_Load(byte[] csvBytes, Action<L10NLangEntity, int, string> cb) {
            Stream stream = new MemoryStream(csvBytes);
            StreamReader sr = new StreamReader(stream);
            CsvReader reader = new CsvReader(sr);
            // 读首行
            reader.Read();
            var dict = new Dictionary<int, L10NLangType>();
            for (int i = 0; i < reader.FieldsCount; i += 1) {
                string key = reader[i];
                for (int j = 0; j < langNames.Length; j += 1) {
                    if (key == langNames[j]) {
                        // 第几列表示哪种语言
                        L10NLangType langType = (L10NLangType)langTypes.GetValue(j);
                        dict.Add(i, langType);
                    }
                }
            }

            // 读每行
            while (reader.Read()) {
                // 读一行:
                // 第0列 ID
                int id = int.Parse(reader[0]);

                // 从第2列开始是文本
                for (int i = 2; i < reader.FieldsCount; i += 1) {
                    bool has = dict.TryGetValue(i, out var langType);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {i}, {reader[i]}; id: {id}");
                        continue;
                    }
                    has = all.TryGetValue(langType, out var entity);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {langType.ToString()}, {reader[i]}; id: {id}");
                        continue;
                    }

                    string text = reader[i];
                    if (string.IsNullOrEmpty(text)) {
                        continue;
                    }

                    // 用 ` 替换逗号
                    text = text.Replace("`", ",");

                    // 用 % 替换引号
                    text = text.Replace("%", "\"");

                    // 用\r\n替换/br
                    text = text.Replace("/br", "\r\n");

                    cb.Invoke(entity, id, text);
                }
            }

            // 释放
            sr.Dispose();
            stream.Dispose();
        }
        #endregion

        #region Load: Word
        void Word_Load(byte[] csvBytes, Action<L10NLangEntity, string, string> cb) {
            Stream stream = new MemoryStream(csvBytes);
            StreamReader sr = new StreamReader(stream);
            CsvReader reader = new CsvReader(sr);
            // 读首行
            reader.Read();
            var dict = new Dictionary<int, L10NLangType>();
            for (int i = 0; i < reader.FieldsCount; i += 1) {
                string key = reader[i];
                for (int j = 0; j < langNames.Length; j += 1) {
                    if (key == langNames[j]) {
                        // 第几列表示哪种语言
                        L10NLangType langType = (L10NLangType)langTypes.GetValue(j);
                        dict.Add(i, langType);
                    }
                }
            }

            // 读每行
            while (reader.Read()) {
                // 读一行:
                // 第0列 ID
                string id = reader[0]; 

                // 从第1列开始是文本
                for (int i = 1; i < reader.FieldsCount; i += 1) {
                    bool has = dict.TryGetValue(i, out var langType);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {langType.ToString()} {i}, {reader[i]}; id: {id}");
                        continue;
                    }
                    has = all.TryGetValue(langType, out var entity);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {langType.ToString()}, {reader[i]}; id: {id}");
                        continue;
                    }

                    string text = reader[i];
                    if (string.IsNullOrEmpty(text)) {
                        continue;
                    }

                    // 用 ` 替换逗号
                    text = text.Replace("`", ",");

                    // 用 % 替换引号
                    text = text.Replace("%", "\"");

                    // 用\r\n替换/br
                    text = text.Replace("/br", "\r\n");

                    cb.Invoke(entity, id, text);
                }
            }

            // 释放
            sr.Dispose();
            stream.Dispose();
        }
        #endregion

        #region Load: Dialogue
        void Dialogue_Load(byte[] csvBytes) {

            // 加载
            Stream stream = new MemoryStream(csvBytes);
            StreamReader sr = new StreamReader(stream);
            CsvReader reader = new CsvReader(sr);

            // 读首行
            reader.Read();
            var dict = new Dictionary<int, L10NLangType>();
            for (int i = 0; i < reader.FieldsCount; i += 1) {
                string key = reader[i];
                for (int j = 0; j < langNames.Length; j += 1) {
                    if (key == langNames[j]) {
                        // 第几列表示哪种语言
                        L10NLangType langType = (L10NLangType)langTypes.GetValue(j);
                        dict.Add(i, langType);
                    }
                }
            }

            // 读每行
            while (reader.Read()) {
                // 读一行:
                // 第0列 TypeID
                int typeID = int.Parse(reader[0]);

                // 第1列 SentenceIndex
                short sentenceIndex = short.Parse(reader[1]);

                // 第2列 OptionIndex
                sbyte optionIndex = sbyte.Parse(reader[2]);

                // 第1列是描述, 可不读

                // 从第2列开始是文本
                for (int i = 3; i < reader.FieldsCount; i += 1) {
                    bool has = dict.TryGetValue(i, out var langType);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {i}, {reader[i]}; id: {typeID}");
                        continue;
                    }
                    has = all.TryGetValue(langType, out var entity);
                    if (!has) {
                        Debug.LogWarning($"L10n.Init: langType not found: {langType.ToString()}, {reader[i]}; id: {typeID}");
                        continue;
                    }
                    string text = reader[i];
                    if (string.IsNullOrEmpty(text)) {
                        continue;
                    }

                    // 用 ` 替换逗号
                    text = text.Replace("`", ",");

                    // 用 % 替换引号
                    text = text.Replace("%", "\"");

                    // 用\r\n替换/br
                    text = text.Replace("/br", "\r\n");

                    entity.Dialogue_Add(typeID, sentenceIndex, optionIndex, text);
                }
            }

            // 释放
            sr.Dispose();
            stream.Dispose();

        }
        #endregion

        L10NLangEntity GetFallbackEntity(L10NLangType l10NLangType) {
            bool has = all.TryGetValue(l10NLangType, out var entity);
            if (!has) {
                has = all.TryGetValue(L10NLangType.EN_US, out entity);
            }
            if (!has) {
                has = all.TryGetValue(L10NLangType.ZH_CN, out entity);
            }
            if (!has) {
                Debug.LogError($"L10n.GetFallbackEntity: langType not found: {l10NLangType}");
            }
            return entity;
        }

        #region Static Normal
        public string Static_Normal_Get(int key) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Static_TryGet(key, out var str);
            if (has) {
                return str;
            } else {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Static_TryGet(key, out str);
                    return str;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Dialogue
        public string Dialogue_GetSentence(int dialogueTypeID, short sentenceIndex) {
            return Dialogue_GetOption(dialogueTypeID, sentenceIndex, 0);
        }

        public string Dialogue_GetOption(int dialogueTypeID, short sentenceIndex, sbyte optionIndex) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Dialogue_TryGet(dialogueTypeID, sentenceIndex, optionIndex, out var str);
            if (has) {
                return str;
            } else {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Dialogue_TryGet(dialogueTypeID, sentenceIndex, optionIndex, out str);
                    return str;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Staff
        public string Staff_Get(int key) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Staff_TryGet(key, out var str);
            if (has) {
                return str;
            } else {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Staff_TryGet(key, out str);
                    return str;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Stuff
        public string Stuff_Get(int key) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Stuff_TryGet(key, out var str);
            if (has) {
                return str;
            } else {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Stuff_TryGet(key, out str);
                    return str;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Doodle
        public string Doodle_Get(int key) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Doodle_TryGet(key, out var str);
            if (has) {
                return str;
            } else {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Doodle_TryGet(key, out str);
                    return str;
                }
            }
            return string.Empty;
        }
        #endregion

        #region Symbol
        public string Symbol_Get(string key) {
            var entity = GetFallbackEntity(currentLangType);
            bool has = entity.Symbol_TryGet(key, out var str);
            if (!has) {
                has = all.TryGetValue(L10NLangType.EN_US, out var enEntity);
                if (has) {
                    enEntity.Symbol_TryGet(key, out str);
                }
            }
            return str;
        }
        #endregion

#if UNITY_EDITOR
        [Button]
        void Combine() {
            List<byte> all = new List<byte>(1000 * 1024);
            List<TextAsset> csvs = new List<TextAsset>();
            csvs.Add(csv_misc);
            csvs.Add(csv_static);
            csvs.Add(csv_dialogue);
            csvs.Add(csv_doodle);
            csvs.Add(csv_word);
            csvs.Add(csv_symbol);
            for (int i = 0; i < csvs.Count; i += 1) {
                TextAsset csv = csvs[i];
                byte[] bytes = csv.bytes;
                all.AddRange(bytes);
            }

            string path = Path.Combine(Application.dataPath, "Res_Runtime", "L10N", "_L10N_Generated.txt");
            File.WriteAllBytes(path, all.ToArray());
        }
#endif
    }

}