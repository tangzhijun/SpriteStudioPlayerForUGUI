﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace a.spritestudio.editor
{
    /// <summary>
    /// 
    /// </summary>
    public class SpriteStudioImporter
        : AssetPostprocessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths )
        {
            foreach ( string str in importedAssets ) {
                if ( str.EndsWith( ".sspj" ) ) {
                    Tracer.enable = true;
                    Tracer.Startup();
                    Tracer.Log( "Start import : " + str );
                    Import( str );
                    Tracer.Dump();
                }
            }
        }

        /// <summary>
        /// インポート
        /// </summary>
        /// <param name="file"></param>
        private static void Import( string file )
        {
            // TODO: 出力先は自由に出来るようにする
            string exportPath = "Assets/Exports/";

            string path = Path.GetDirectoryName( file );

            // マテリアル
            Dictionary<types.AlphaBlendType, Material> materials = new Dictionary<types.AlphaBlendType, Material>() {
                { types.AlphaBlendType.kAdd, AssetDatabase.LoadAssetAtPath( "Assets/SpriteStudioPlayer/Materials/Add.mat", typeof( Material ) ) as Material },
                { types.AlphaBlendType.kMix, AssetDatabase.LoadAssetAtPath( "Assets/SpriteStudioPlayer/Materials/Mix.mat", typeof( Material ) ) as Material },
                { types.AlphaBlendType.kMul, AssetDatabase.LoadAssetAtPath( "Assets/SpriteStudioPlayer/Materials/Mul.mat", typeof( Material ) ) as Material },
                { types.AlphaBlendType.kSub, AssetDatabase.LoadAssetAtPath( "Assets/SpriteStudioPlayer/Materials/Sub.mat", typeof( Material ) ) as Material },
            };

            // sspjのインポート
            var projectInformation = new SSPJImporter().Import( file );
            Tracer.Log( projectInformation.ToString() );

            // ssceのインポート
            List<CellMap> cellMap = new List<CellMap>();
            foreach ( var cell in projectInformation.cellMaps ) {
                // ssceをパース
                var importer = new SSCEImporter();
                var ssceInformation = importer.Import( path + '\\' + cell );
                Tracer.Log( ssceInformation.ToString() );

                // エンジン側の形式へ変更
                var converter = new SSCEConverter();
                cellMap.Add( converter.Convert( path + '\\', ssceInformation ) );
            }
            
            // セルマップの保存
            CreateFolders( exportPath + "CellMaps" );
            for ( int i = 0; i < cellMap.Count; ++i ) {
                var cell = cellMap[i];
                string fileName = exportPath + "CellMaps/" + cell.name + ".asset";
                AssetDatabase.CreateAsset( cell, fileName );
                cellMap[i] = (CellMap) AssetDatabase.LoadAssetAtPath( fileName, typeof( CellMap ) );

                Tracer.Log( "Save CellMap:" + fileName );
            }

            // ssaeのインポート
            List<GameObject> prefabs = new List<GameObject>();
            try {
                string basePath = exportPath + "Sprites/" + Path.GetFileNameWithoutExtension( file );
                foreach ( var animation in projectInformation.animePacks ) {
                    // ssaeをパース
                    var ssaeInformation = new SSAEImporter().Import( path + '\\' + animation );
                    Tracer.Log( ssaeInformation.ToString() );

                    // GameObjectへ変換
                    var converter = new SSAEConverter();
                    var result = converter.Convert( projectInformation,
                            ssaeInformation, cellMap, materials );
                    prefabs.AddRange( result.animations );

                    // prefab保存
                    string name = Path.GetFileNameWithoutExtension( animation );
                    CreateFolders( basePath + "/" + name );
                    foreach ( var prefab in result.animations ) {
                        string fileName = basePath + "/" + name + "/" + prefab.name + ".prefab";
                        PrefabUtility.CreatePrefab( fileName, prefab );

                        Tracer.Log( "Save Prefab:" + fileName );
                    }
                }
            } finally {
                // ヒエラルキーにGOが残らないようにする
                if ( prefabs != null ) {
                    foreach ( var p in prefabs ) {
                        GameObject.DestroyImmediate( p );
                    }
                }
                AssetDatabase.SaveAssets();
            }
        }

        /// <summary>
        /// フォルダ生成
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolders( string path )
        {
            Directory.CreateDirectory( path );
        }
    }
}