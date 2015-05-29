﻿using UnityEditor;

namespace a.spritestudio.editor
{
    /// <summary>
    /// メニューに出す項目
    /// </summary>
    public class MenuItems
    {
        /// <summary>
        /// メニューの順番
        /// </summary>
        private enum Order
        {
            kAutoImport,
            kImportLog,
        }

        #region 自動インポートの設定

        /// <summary>
        /// 自動インポート
        /// </summary>
        public static bool AutoImport
        {
            get { return EditorPrefs.GetBool( "assfugui-ai", false ); }
            set { EditorPrefs.SetBool( "assfugui-ai", value ); }
        }

        /// <summary>
        /// 順番
        /// </summary>
        /// <returns></returns>
        [MenuItem( "SpriteStudioForUGUI/自動インポート", true, (int) Order.kAutoImport )]
        public static bool EnableAutoImportOrder()
        {
            return true;
        }

        /// <summary>
        /// 自動インポート有り
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/自動インポート/On", false, 1 )]
        public static void EnableAutoImport()
        {
            AutoImport = true;
        }

        /// <summary>
        /// 自動インポート有り
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/自動インポート/On", true, 1 )]
        public static bool IsValidEnableAutoImport()
        {
            return !AutoImport;
        }

        /// <summary>
        /// 自動インポートなし
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/自動インポート/Off", false, 2 )]
        public static void DisableAutoImport()
        {
            AutoImport = false;
        }

        /// <summary>
        /// 自動インポートなし
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/自動インポート/Off", true, 2 )]
        public static bool IsValidDisableAutoImport()
        {
            return AutoImport;
        }
        #endregion

        #region インポートログの有無
        /// <summary>
        /// 自動インポート
        /// </summary>
        public static bool ImportLog
        {
            get { return EditorPrefs.GetBool( "assfugui-il", false ); }
            set { EditorPrefs.SetBool( "assfugui-il", value ); }
        }

        /// <summary>
        /// 順番
        /// </summary>
        /// <returns></returns>
        [MenuItem( "SpriteStudioForUGUI/インポートログ", true, (int) Order.kImportLog )]
        public static bool EnableImportLogOrder()
        {
            return true;
        }


        /// <summary>
        /// 自動インポート有り
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/インポートログ/On", false, 1 )]
        public static void EnableImportLog()
        {
            ImportLog = true;
        }

        /// <summary>
        /// 自動インポート有り
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/インポートログ/On", true, 1 )]
        public static bool IsValidEnableImportLog()
        {
            return !ImportLog;
        }

        /// <summary>
        /// 自動インポートなし
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/インポートログ/Off", false, 2 )]
        public static void DisableImportLog()
        {
            ImportLog = false;
        }

        /// <summary>
        /// 自動インポートなし
        /// </summary>
        [MenuItem( "SpriteStudioForUGUI/インポートログ/Off", true, 2 )]
        public static bool IsValidDisableImportLog()
        {
            return ImportLog;
        }
        #endregion
    }
}