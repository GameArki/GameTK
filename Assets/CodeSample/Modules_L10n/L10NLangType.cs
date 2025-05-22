namespace NJM {

/* Steam 支持的语言列表
中文名				原文名					API 语言代码		Web API 语言代码
阿拉伯语 *			العربية					arabic				ar
保加利亚语			български език			bulgarian			bg
简体中文			简体中文				schinese			zh-CN
繁体中文			繁體中文				tchinese			zh-TW
捷克语				čeština					czech				cs
丹麦语				Dansk					danish				da
荷兰语				Nederlands				dutch				nl
英语				English					english				en
芬兰语				Suomi					finnish				fi
法语				Français				french				fr
德语				Deutsch					german				de
希腊语				Ελληνικά				greek				el
匈牙利语			Magyar					hungarian			hu
印度尼西亚语		Bahasa Indonesia		indonesian			id
意大利语			Italiano				italian				it
日语				日本語					japanese			ja
韩语				한국어					koreana				ko
挪威语				Norsk					norwegian			no
波兰语				Polski					polish				pl
葡萄牙语			Português				portuguese			pt
葡萄牙语 - 巴西		Português-Brasil		brazilian			pt-BR
罗马尼亚语			Română					romanian			ro
俄语				Русский					russian				ru
西班牙语 - 西班牙	Español-España			spanish				es
西班牙语 - 拉丁美洲	Español-Latinoamérica	latam				es-419
瑞典语				Svenska					swedish				sv
泰语				ไทย						thai				th
土耳其语			Türkçe					turkish				tr
乌克兰语			Українська				ukrainian			uk
越南语				Tiếng Việt				vietnamese			vi
*/

    public enum L10NLangType {
        ZH_CN = 0,
        ZH_TW = 1,
        EN_US = 2,
        JA = 3,
    }

    public static class L10NLangTypeExtension {
        public static string ToVDFStr(this L10NLangType lang) {
            switch (lang) {
                case L10NLangType.ZH_CN:
                    return "schinese";
                case L10NLangType.ZH_TW:
                    return "tchinese";
                case L10NLangType.EN_US:
                    return "english";
                case L10NLangType.JA:
                    return "japanese";
                default:
                    UnityEngine.Debug.LogError("unknown lang type: " + lang.ToString());
                    return "schinese";
            }
        }
    }

}
